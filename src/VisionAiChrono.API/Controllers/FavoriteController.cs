using MediatR;
using Microsoft.AspNetCore.Mvc;
using VisionAiChrono.Application.Dtos;
using VisionAiChrono.Application.Dtos.FavoriteDtos;
using VisionAiChrono.Application.Slices.Commands.FavoriteCommand;
using VisionAiChrono.Application.Slices.Queries.FavoriteQueries;

namespace VisionAiChrono.API.Controllers
{
    /// <summary>
    /// Controller for managing user favorites.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class FavoriteController(ISender sender, ILogger<FavoriteController> logger)
        : ControllerBase
    {
        /// <summary>
        /// Retrieves a specific favorite by its unique identifier.
        /// </summary>
        /// <param name="favoriteId">The unique identifier of the favorite.</param>
        /// <returns>An ApiResponse containing the favorite details if found.</returns>
        /// <response code="200">Returns the favorite details.</response>
        /// <response code="404">If the favorite is not found.</response>
        [HttpGet("{favoriteId}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> GetFavoriteByIdAsync(Guid favoriteId)
        {
            logger.LogInformation("Received request to get favorite with Id: {FavoriteId}", favoriteId);

            var result = await sender.Send(new GetFavoriteByIdQuery(favoriteId));
            if (result == null)
            {
                logger.LogWarning("Favorite with Id: {FavoriteId} not found", favoriteId);
                return NotFound(new ApiResponse
                {
                    IsSuccess = false,
                    Message = "Favorite not found",
                    StatusCode = System.Net.HttpStatusCode.NotFound
                });
            }
            return Ok(new ApiResponse
            {
                Message = "Favorite retrieved successfully",
                StatusCode = System.Net.HttpStatusCode.OK,
                IsSuccess = true,
                Result = result
            });
        }

        /// <summary>
        /// Retrieves all favorites for a specific user.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>An ApiResponse containing a list of the user's favorites.</returns>
        /// <response code="200">Returns the list of favorites.</response>
        [HttpGet("user/{userId}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse>> GetFavoritesByUserIdAsync(Guid userId)
        {
            logger.LogInformation("Received request to get favorites for UserId: {UserId}", userId);
            var result = await sender.Send(new GetFavoritesByQuery(x => x.UserId == userId));
            return Ok(new ApiResponse
            {
                Message = "Favorites retrieved successfully",
                StatusCode = System.Net.HttpStatusCode.OK,
                IsSuccess = true,
                Result = result
            });
        }

        /// <summary>
        /// Adds a new favorite for a user.
        /// </summary>
        /// <param name="request">The favorite creation request containing user and pipeline identifiers.</param>
        /// <returns>An ApiResponse containing the created favorite details.</returns>
        /// <response code="200">Returns the created favorite.</response>
        /// <response code="400">If the request is invalid.</response>
        [HttpPost("add")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ApiResponse>> AddFavoriteAsync([FromBody] FavoriteAddRequest request)
        {
            logger.LogInformation("Received request to add favorite for UserId: {UserId}, PipelineId: {PipelineId}", request.UserId, request.PipeleineId);
            var result = await sender.Send(new AddFavoriteCommand(request));
            return Ok(new ApiResponse
            {
                Message = "Favorite added successfully",
                StatusCode = System.Net.HttpStatusCode.OK,
                IsSuccess = true,
                Result = result
            });
        }

        /// <summary>
        /// Deletes a favorite by its unique identifier.
        /// </summary>
        /// <param name="favId">The unique identifier of the favorite to delete.</param>
        /// <returns>An ApiResponse indicating the result of the deletion.</returns>
        /// <response code="200">If the favorite was successfully deleted.</response>
        /// <response code="404">If the favorite is not found.</response>
        [HttpDelete("{favId}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ApiResponse>> DeleteFavoriteAsync(Guid favId)
        {
            logger.LogInformation("Received request to delete favorite with Id: {FavoriteId}", favId);
            var result = await sender.Send(new DeleteFavoriteCommand(favId));
            if (!result)
            {
                logger.LogWarning("Favorite with Id: {FavoriteId} not found for deletion", favId);
                return NotFound(new ApiResponse
                {
                    IsSuccess = false,
                    Message = "Favorite not found",
                    StatusCode = System.Net.HttpStatusCode.NotFound
                });
            }
            return Ok(new ApiResponse
            {
                Message = "Favorite deleted successfully",
                StatusCode = System.Net.HttpStatusCode.OK,
                IsSuccess = true,
                Result = result
            });
        }
    }
}