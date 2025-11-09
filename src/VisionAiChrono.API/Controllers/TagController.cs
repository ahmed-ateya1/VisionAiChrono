using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VisionAiChrono.Application.Dtos;
using VisionAiChrono.Application.Dtos.TagDtos;
using VisionAiChrono.Application.Dtos.VideoTagDtos;
using VisionAiChrono.Application.Slices.Commands.TagCommand;
using VisionAiChrono.Application.Slices.Commands.TagCommand.AddTagForVideo;
using VisionAiChrono.Application.Slices.Queries.TagQueries;

namespace VisionAiChrono.API.Controllers
{
    /// <summary>
    /// Controller for managing tags and video-tag associations.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TagController(ISender sender , ILogger<TagController> logger) 
        : ControllerBase
    {
        /// <summary>
        /// Retrieves all tags in the system.
        /// </summary>
        /// <returns>An ApiResponse containing the list of all tags.</returns>
        /// <response code="200">Returns the list of tags.</response>
        /// <response code="404">If no tags are found.</response>
        [HttpGet("get-all")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetAllTags()
        {
            var tags = await sender.Send(new GetTagsByQuery(null));

            if(tags == null || !tags.Any())
            {
                logger.LogWarning("No tags found");
                return NotFound(new ApiResponse
                {
                    IsSuccess = false,
                    Message = "No tags found",
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Result = null
                });
            }
            logger.LogInformation("Retrieved {TagCount} tags", tags?.Count() ?? 0);
            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Message = "Tags retrieved successfully",
                StatusCode = System.Net.HttpStatusCode.OK,
                Result = tags
            });
        }

        /// <summary>
        /// Retrieves a specific tag by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the tag.</param>
        /// <returns>An ApiResponse containing the tag details if found.</returns>
        /// <response code="200">Returns the tag details.</response>
        /// <response code="404">If the tag is not found.</response>
        [HttpGet("get-by-id/{id}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetTagById(Guid id)
        {
            var tag = await sender.Send(new GetTagsByQuery(x => x.Id == id));
            if (tag == null)
            {
                logger.LogWarning("Tag with ID {TagId} not found", id);
                return NotFound(new ApiResponse
                {
                    IsSuccess = false,
                    Message = "Tag not found",
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Result = null
                });
            }
            logger.LogInformation("Tag with ID {TagId} retrieved successfully", id);
            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Message = "Tag retrieved successfully",
                StatusCode = System.Net.HttpStatusCode.OK,
                Result = tag
            });
        }

        /// <summary>
        /// Retrieves tags that match the specified name (case-insensitive partial match).
        /// </summary>
        /// <param name="name">The name or partial name to search for.</param>
        /// <returns>An ApiResponse containing the list of matching tags.</returns>
        /// <response code="200">Returns the list of matching tags.</response>
        /// <response code="404">If no matching tags are found.</response>
        [HttpGet("get-by-name/{name}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetTagByName(string name)
        {

            var tags = await sender.Send(new GetTagsByQuery(x => x.Name.ToUpper().Contains(name.ToUpper())));
            if (tags == null || !tags.Any())
            {
                logger.LogWarning("No tags found");
                return NotFound(new ApiResponse
                {
                    IsSuccess = false,
                    Message = "No tags found",
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Result = null
                });
            }
            logger.LogInformation("Tag with name {TagName} retrieved successfully", name);
            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Message = "Tag retrieved successfully",
                StatusCode = System.Net.HttpStatusCode.OK,
                Result = tags
            });
        }

        /// <summary>
        /// Retrieves all tags created by a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>An ApiResponse containing the list of tags for the specified user.</returns>
        /// <response code="200">Returns the list of user's tags.</response>
        /// <response code="404">If no tags are found for the user.</response>
        [HttpGet("get-tags-by-user/{userId}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetTagsByUserId(Guid userId)
        {
            var tags = await sender.Send(new GetTagsByQuery(x => x.UserId == userId));
            if (tags == null || !tags.Any())
            {
                logger.LogWarning("No tags found for user ID {UserId}", userId);
                return NotFound(new ApiResponse
                {
                    IsSuccess = false,
                    Message = "No tags found for the specified user",
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Result = null
                });
            }
            logger.LogInformation("Retrieved {TagCount} tags for user ID {UserId}", tags.Count(), userId);
            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Message = "Tags retrieved successfully",
                StatusCode = System.Net.HttpStatusCode.OK,
                Result = tags
            });
        }

        /// <summary>
        /// Associates a tag with a video.
        /// </summary>
        /// <param name="request">The video-tag association request containing video and tag identifiers.</param>
        /// <returns>An ApiResponse containing the created video-tag association details.</returns>
        /// <response code="200">Returns the created video-tag association.</response>
        /// <response code="400">If the request is invalid or the operation fails.</response>
        [HttpPost("add-video-tag")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> AddTagForVideo(VideoTagAddRequest request)
        {
            var response = await sender.Send(new AddTagForVideoCommand(request));
            if (response == null)
            {
                return BadRequest(new ApiResponse
                {
                    IsSuccess = false,
                    Message = "failed to add tag to video",
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                });
            }
            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Message = "Tag Added to Video Sucessfully",
                StatusCode = System.Net.HttpStatusCode.OK,
                Result = response
            });
        }

        /// <summary>
        /// Creates a new tag.
        /// </summary>
        /// <param name="request">The tag creation request containing tag details.</param>
        /// <returns>An ApiResponse containing the created tag details.</returns>
        /// <response code="200">Returns the created tag.</response>
        /// <response code="400">If the request is invalid or the operation fails.</response>
        /// <response code="401">If the user is not authenticated.</response>
        [HttpPost("add-tag")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ApiResponse>> AddTag(TagAddRequest request)
        {

            var response = await sender.Send(new CreateTagCommand(request));

            if(response == null)
            {
                return BadRequest(new ApiResponse
                {
                    IsSuccess = false,
                    Message = "failed to add tag",
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                });
            }

            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Message = "Tag Added Sucessfully",
                StatusCode = System.Net.HttpStatusCode.OK,
                Result = response
            });
        }

        /// <summary>
        /// Updates an existing tag.
        /// </summary>
        /// <param name="request">The tag update request containing the tag identifier and updated details.</param>
        /// <returns>An ApiResponse containing the updated tag details.</returns>
        /// <response code="200">Returns the updated tag.</response>
        /// <response code="400">If the request is invalid or the operation fails.</response>
        /// <response code="401">If the user is not authenticated.</response>
        [HttpPut("update-tag")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<ApiResponse>> UpdateTag(TagUpdateRequest request)
        {

            var response = await sender.Send(new UpdateTagCommand(request));

            if (response == null)
            {
                return BadRequest(new ApiResponse
                {
                    IsSuccess = false,
                    Message = "failed to update tag",
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                });
            }

            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Message = "Tag Updated Sucessfully",
                StatusCode = System.Net.HttpStatusCode.OK,
                Result = response
            });
        }
        

    }
}
