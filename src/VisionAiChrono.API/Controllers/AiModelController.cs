using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using VisionAiChrono.Application.Dtos;
using VisionAiChrono.Application.Dtos.AiModelDtos;
using VisionAiChrono.Application.Slices.Commands;
using VisionAiChrono.Application.Slices.Queries;
using VisionAiChrono.Domain.Models;

namespace VisionAiChrono.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AiModelController(IMediator mediator, ILogger<AiModelController> logger)
        : ControllerBase
    {
        /// <summary>
        /// Retrieves all available AI models with pagination support.
        /// </summary>
        /// <param name="pagination">Pagination and sorting options.</param>
        /// <returns>List of AI models with pagination metadata.</returns>
        /// <response code="200">AI models retrieved successfully.</response>
        /// <response code="404">No AI models found.</response>
        [HttpGet("get-all")]
        public async Task<ActionResult<ApiResponse>> GetAllAiModels([FromQuery] PaginationDto pagination)
        {
            logger.LogInformation(
                "Fetching all AI models with pagination: PageIndex={PageIndex}, PageSize={PageSize}",
                pagination.PageIndex,
                pagination.PageSize);

            var response = await mediator.Send(new GetAllModelsQuery(pagination));

            if (response == null || !response.Items.Any())
            {
                logger.LogWarning("No AI models found in database.");
                return NotFound(new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Message = "No AI models found."
                });
            }

            logger.LogInformation("Retrieved {Count} AI models successfully.", response.Items.Count());
            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Message = "AI models retrieved successfully.",
                Result = response,
                StatusCode = System.Net.HttpStatusCode.OK
            });
        }

        /// <summary>
        /// Retrieves AI models filtered by name with pagination.
        /// </summary>
        /// <param name="name">The AI model name or part of it to filter by.</param>
        /// <param name="pagination">Pagination and sorting options.</param>
        /// <returns>List of AI models matching the specified name.</returns>
        /// <response code="200">AI models retrieved successfully.</response>
        /// <response code="404">No AI models found matching the given name.</response>
        [HttpGet("get-all-by-name/{name}")]
        public async Task<ActionResult<ApiResponse>> GetAllByName(string name, [FromQuery] PaginationDto pagination)
        {
            logger.LogInformation("Fetching AI models filtered by name: {Name}", name);

            Expression<Func<AiModel, bool>> filter = x =>
                x.Name.ToLower().Contains(name.ToLower());

            var response = await mediator.Send(new GetAllModelsByQuery(pagination, filter));

            if (response == null || !response.Items.Any())
            {
                logger.LogWarning("No AI models found containing name: {Name}", name);
                return NotFound(new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Message = $"No AI models found with name containing '{name}'."
                });
            }

            logger.LogInformation("Retrieved {Count} AI models matching name '{Name}'.", response.Items.Count(), name);
            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Message = "AI models retrieved successfully.",
                Result = response,
                StatusCode = System.Net.HttpStatusCode.OK
            });
        }

        /// <summary>
        /// Retrieves AI models filtered by their active status with pagination.
        /// </summary>
        /// <param name="isActive">Specifies whether to fetch active (true) or inactive (false) AI models.</param>
        /// <param name="pagination">Pagination and sorting options.</param>
        /// <returns>List of AI models filtered by active status.</returns>
        /// <response code="200">AI models retrieved successfully.</response>
        /// <response code="404">No AI models found with the specified active status.</response>

        [HttpGet("get-all-by-active/{isActive}")]
        public async Task<ActionResult<ApiResponse>> GetAllByActive(bool isActive, [FromQuery] PaginationDto pagination)
        {
            logger.LogInformation("Fetching AI models filtered by active status: {IsActive}", isActive);
            Expression<Func<AiModel, bool>> filter = x =>
                x.IsActive == isActive;
            var response = await mediator.Send(new GetAllModelsByQuery(pagination, filter));
            if (response == null || !response.Items.Any())
            {
                logger.LogWarning("No AI models found with active status: {IsActive}", isActive);
                return NotFound(new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Message = $"No AI models found with active status '{isActive}'."
                });
            }
            logger.LogInformation("Retrieved {Count} AI models with active status '{IsActive}'.", response.Items.Count(), isActive);
            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Message = "AI models retrieved successfully.",
                Result = response,
                StatusCode = System.Net.HttpStatusCode.OK
            });
        }

        /// <summary>
        /// Retrieves AI models filtered by version string with pagination.
        /// </summary>
        /// <param name="version">The version (or part of it) to filter AI models by.</param>
        /// <param name="pagination">Pagination and sorting options.</param>
        /// <returns>List of AI models matching the specified version.</returns>
        /// <response code="200">AI models retrieved successfully.</response>
        /// <response code="404">No AI models found matching the specified version.</response>
        [HttpGet("get-all-by-version/{version}")]
        public async Task<ActionResult<ApiResponse>> GetAllByVersion(string version, [FromQuery] PaginationDto pagination)
        {
            logger.LogInformation("Fetching AI models filtered by version: {Version}", version);
            Expression<Func<AiModel, bool>> filter = x =>
                x.Version.ToLower().Contains(version.ToLower());
            var response = await mediator.Send(new GetAllModelsByQuery(pagination, filter));
            if (response == null || !response.Items.Any())
            {
                logger.LogWarning("No AI models found containing version: {Version}", version);
                return NotFound(new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Message = $"No AI models found with version containing '{version}'."
                });
            }
            logger.LogInformation("Retrieved {Count} AI models matching version '{Version}'.", response.Items.Count(), version);
            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Message = "AI models retrieved successfully.",
                Result = response,
                StatusCode = System.Net.HttpStatusCode.OK
            });


        }
        /// <summary>
        /// Retrieves a single AI model by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier (GUID) of the AI model to retrieve.</param>
        /// <returns>The AI model that matches the specified ID.</returns>
        /// <response code="200">AI model retrieved successfully.</response>
        /// <response code="404">No AI model found with the specified ID.</response>
        [HttpGet("get-by-id/{id}")]
        public async Task<ActionResult<ApiResponse>> GetById(Guid id)
        {
            logger.LogInformation("Fetching AI model by ID: {Id}", id);
            Expression<Func<AiModel, bool>> filter = x =>
                x.Id == id;
            var response = await mediator.Send(new GetAllModelsByQuery(null, filter));
            var model = response.Items.FirstOrDefault();
            if (model == null)
            {
                logger.LogWarning("No AI model found with ID: {Id}", id);
                return NotFound(new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Message = $"No AI model found with ID '{id}'."
                });
            }
            logger.LogInformation("Retrieved AI model with ID '{Id}'.", id);
            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Message = "AI model retrieved successfully.",
                Result = model,
                StatusCode = System.Net.HttpStatusCode.OK
            });
        }
        /// <summary>
        /// Creates a new AI model.
        /// </summary>
        /// <param name="modelCreate">The details of the AI model to create.</param>
        /// <returns>The newly created AI model.</returns>
        /// <response code="200">AI model created successfully.</response>
        /// <response code="400">Validation error occurred while creating the AI model.</response>
        [HttpPost("create")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ApiResponse>> CreateAiModel([FromBody] ModelAddRequest modelCreate)
        {
            logger.LogInformation("Creating a new AI model with name: {Name}", modelCreate.Name);
            var response = await mediator.Send(new AddAiModelCommand(modelCreate));
            logger.LogInformation("AI model created successfully with ID: {Id}", response.Id);
            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Message = "AI model created successfully.",
                Result = response,
                StatusCode = System.Net.HttpStatusCode.OK
            });
        }
        /// <summary>
        /// Updates an existing AI model.
        /// </summary>
        /// <param name="modelUpdate">The updated details of the AI model.</param>
        /// <returns>The updated AI model.</returns>
        /// <response code="200">AI model updated successfully.</response>
        /// <response code="404">No AI model found with the specified ID.</response>
        /// <response code="400">Validation error occurred while updating the AI model.</response>
        [HttpPut("update")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ApiResponse>> UpdateAiModel([FromBody] ModelUpdateRequest modelUpdate)
        {
            logger.LogInformation("Updating AI model with ID: {Id}", modelUpdate.Id);
            var response = await mediator.Send(new UpdateAiModelCommand(modelUpdate));
            logger.LogInformation("AI model with ID: {Id} updated successfully.", response.Id);
            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Message = "AI model updated successfully.",
                Result = response,
                StatusCode = System.Net.HttpStatusCode.OK
            });
        }
        /// <summary>
        /// Deletes an AI model by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier (GUID) of the AI model to delete.</param>
        /// <returns>Boolean result indicating whether the deletion was successful.</returns>
        /// <response code="200">AI model deleted successfully.</response>
        /// <response code="404">No AI model found with the specified ID.</response>
        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ApiResponse>> DeleteAiModel(Guid id)
        {
            logger.LogInformation("Deleting AI model with ID: {Id}", id);
            Expression<Func<AiModel, bool>> filter = x =>
                x.Id == id;
            var response = await mediator.Send(new DeleteAiModelCommand(id));
            
            if (response == false)
            {
                logger.LogWarning("No AI model found with ID: {Id}", id);
                return NotFound(new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Message = $"No AI model found with ID '{id}'."
                });
            }
            logger.LogInformation("AI model with ID: {Id} deleted successfully.", id);
            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Message = "AI model deleted successfully.",
                Result = true,
                StatusCode = System.Net.HttpStatusCode.OK
            });
        }
    }
}
