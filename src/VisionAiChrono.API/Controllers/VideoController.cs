using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Net;
using VisionAiChrono.Application.Dtos;
using VisionAiChrono.Application.Dtos.VideoDtos;
using VisionAiChrono.Application.Slices.Commands.VideoCommand;
using VisionAiChrono.Application.Slices.Queries.VideoQueries;
using VisionAiChrono.Domain.Models;

namespace VisionAiChrono.API.Controllers
{
    /// <summary>
    /// Controller responsible for managing video-related operations such as adding, retrieving, and deleting videos.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class VideoController(ISender sender, ILogger<VideoController> logger)
        : ControllerBase
    {
        /// <summary>
        /// Retrieves a video by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the video.</param>
        /// <returns>An <see cref="ApiResponse"/> containing the video details if found; otherwise, a not found response.</returns>
        /// <response code="200">Returns the video details successfully.</response>
        /// <response code="404">Video not found.</response>
        [HttpGet("get-by-id/{id}")]
        public async Task<ActionResult<ApiResponse>> GetVideoById(Guid id)
        {
            var video = await sender.Send(new GetVideoByIdQuery(id));
            if (video == null)
            {
                logger.LogWarning("Video with ID {VideoId} not found", id);
                return NotFound(new ApiResponse
                {
                    IsSuccess = false,
                    Message = "Video not found",
                    StatusCode = HttpStatusCode.NotFound
                });
            }

            logger.LogInformation("Video with ID {VideoId} retrieved successfully", id);
            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Message = "Video retrieved successfully",
                StatusCode = HttpStatusCode.OK,
                Result = video
            });
        }

        /// <summary>
        /// Retrieves all videos available in the system.
        /// </summary>
        /// <returns>An <see cref="ApiResponse"/> containing a list of videos.</returns>
        /// <response code="200">Returns all videos successfully, or an empty list if none exist.</response>
        [HttpGet("get-all")]
        public async Task<ActionResult<ApiResponse>> GetAllVideos()
        {
            var videos = await sender.Send(new GetVideosByQuery());
            logger.LogInformation("Retrieved all videos, count: {VideoCount}", videos.Items?.Count() ?? 0);

            if (videos.Items == null || !videos.Items.Any())
            {
                logger.LogInformation("No videos found in the system.");
                return Ok(new ApiResponse
                {
                    IsSuccess = true,
                    Message = "No videos found",
                    StatusCode = HttpStatusCode.OK,
                    Result = Enumerable.Empty<VideoResponse>()
                });
            }

            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Message = "Videos retrieved successfully",
                StatusCode = HttpStatusCode.OK,
                Result = videos.Items
            });
        }

        /// <summary>
        /// Retrieves videos whose title contains the specified text.
        /// </summary>
        /// <param name="title">The title or partial title of the video to search for.</param>
        /// <returns>An <see cref="ApiResponse"/> containing a list of matching videos.</returns>
        /// <response code="200">Returns matching videos successfully, or an empty list if none are found.</response>
        [HttpGet("get-all-by-title/{title}")]
        public async Task<ActionResult<ApiResponse>> GetVideosByName(string title)
        {
            Expression<Func<Video, bool>> expression = x => x.Title.ToUpper().Contains(title.ToUpper());
            var videos = await sender.Send(new GetVideosByQuery(expression));

            logger.LogInformation("Retrieved videos by name '{VideoTitle}', count: {VideoCount}", title, videos.Items?.Count() ?? 0);

            if (videos.Items == null || !videos.Items.Any())
            {
                logger.LogInformation("No videos found in the system.");
                return Ok(new ApiResponse
                {
                    IsSuccess = true,
                    Message = "No videos found",
                    StatusCode = HttpStatusCode.OK,
                    Result = Enumerable.Empty<VideoResponse>()
                });
            }

            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Message = "Videos retrieved successfully",
                StatusCode = HttpStatusCode.OK,
                Result = videos.Items
            });
        }

        /// <summary>
        /// Retrieves all videos uploaded by a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>An <see cref="ApiResponse"/> containing a list of videos uploaded by the user.</returns>
        /// <response code="200">Returns the user's videos successfully, or an empty list if none are found.</response>
        [HttpGet("get-all-by/{userId}")]
        public async Task<ActionResult<ApiResponse>> GetVideosByUserId(Guid userId)
        {
            Expression<Func<Video, bool>> expression = x => x.UserId == userId;
            var videos = await sender.Send(new GetVideosByQuery(expression));

            logger.LogInformation("Retrieved videos by user ID '{UserId}', count: {VideoCount}", userId, videos.Items?.Count() ?? 0);

            if (videos.Items == null || !videos.Items.Any())
            {
                logger.LogInformation("No videos found for user ID {UserId}.", userId);
                return Ok(new ApiResponse
                {
                    IsSuccess = true,
                    Message = "No videos found",
                    StatusCode = HttpStatusCode.OK,
                    Result = Enumerable.Empty<VideoResponse>()
                });
            }

            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Message = "Videos retrieved successfully",
                StatusCode = HttpStatusCode.OK,
                Result = videos.Items
            });
        }

        /// <summary>
        /// Adds a new video to the system.
        /// </summary>
        /// <param name="request">The video data to be added, including title, file, and metadata.</param>
        /// <returns>An <see cref="ApiResponse"/> indicating the result of the operation.</returns>
        /// <response code="200">Video added successfully.</response>
        /// <response code="500">Failed to add the video due to a server error.</response>
        [HttpPost("add")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> AddVideo([FromForm] VideoAddRequest request)
        {
            var response = await sender.Send(new AddVideoCommand(request));

            if (response == null)
            {
                logger.LogError("Failed to add video: response is null");
                return StatusCode(500, new ApiResponse
                {
                    IsSuccess = false,
                    Message = "Failed to add video",
                    StatusCode = HttpStatusCode.InternalServerError
                });
            }

            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Message = "Success to add video",
                StatusCode = HttpStatusCode.OK,
                Result = response
            });
        }

        /// <summary>
        /// Deletes a video by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the video to be deleted.</param>
        /// <returns>An <see cref="ApiResponse"/> indicating the result of the deletion operation.</returns>
        /// <response code="200">Video deleted successfully.</response>
        /// <response code="404">Video not found.</response>
        [HttpDelete("delete/{id}")]
        [Authorize]
        public async Task<ActionResult<ApiResponse>> DeleteVideo(Guid id)
        {
            var response = await sender.Send(new DeleteVideoCommand(id));
            if (!response)
            {
                logger.LogWarning("Video with ID {VideoId} not found for deletion", id);
                return NotFound(new ApiResponse
                {
                    IsSuccess = false,
                    Message = "Video not found",
                    StatusCode = HttpStatusCode.NotFound
                });
            }

            logger.LogInformation("Video with ID {VideoId} deleted successfully", id);
            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Message = "Video deleted successfully",
                StatusCode = HttpStatusCode.OK
            });
        }
    }
}
