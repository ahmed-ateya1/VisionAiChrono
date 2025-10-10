using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using VisionAiChrono.Application.Dtos;
using VisionAiChrono.Application.Dtos.PipelineDtos;
using VisionAiChrono.Application.Slices.Commands.PipelineCommand;
using VisionAiChrono.Application.Slices.Queries.PipelineQueries;

namespace VisionAiChrono.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PipelineController (IMediator mediator , ILogger<PipelineController> logger)
        : ControllerBase
    {
        /// <summary>
        /// Retrieves all available pipelines with pagination support.
        /// </summary>
        /// <param name="pagination">Pagination and sorting options.</param>
        /// <returns>List of pipelines with pagination metadata.</returns>
        /// <response code="200">Pipelines retrieved successfully.</response>
        /// <response code="404">No pipelines found.</response>
        [HttpGet("get-all")]
        public async Task<ActionResult<ApiResponse>> GetPipelines([FromQuery] PaginationDto pagination)
        {
            var pipelines = await mediator.Send(new GetPipelinesQuery(pagination));

            if (pipelines.Items == null || !pipelines.Items.Any())
            {
                logger.LogWarning("No pipelines found.");
                return NotFound(new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Message = "No pipelines found."
                });
            }

            logger.LogInformation("Retrieved {Count} pipelines successfully.", pipelines.Items.Count());
            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Message = "Pipelines retrieved successfully.",
                Result = pipelines,
                StatusCode = System.Net.HttpStatusCode.OK
            });
        }
        /// <summary>
        /// Retrieves pipelines filtered by title with pagination support.
        /// </summary>
        /// <param name="title">The pipeline title or part of it to filter by.</param>
        /// <param name="pagination">Pagination and sorting options.</param>
        /// <returns>List of pipelines matching the specified title.</returns>
        /// <response code="200">Pipelines retrieved successfully.</response>
        /// <response code="404">No pipelines found matching the given title.</response>
        [HttpGet("get-by-title/{title}")]
        public async Task<ActionResult<ApiResponse>> GetPipelineByTitle([FromQuery]string title , [FromQuery] PaginationDto pagination)
        {
            Expression<Func<Domain.Models.Pipeline, bool>> filter
                = pi=>pi.Title.ToLower().Contains(title.ToLower());


            var pipelines = await mediator.Send(new GetPipelinesByQuery(
                filter,
                pagination
                ));

            if (pipelines.Items == null || !pipelines.Items.Any())
            {
                logger.LogWarning("No pipelines found.");
                return NotFound(new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Message = "No pipelines found."
                });
            }

            logger.LogInformation("Retrieved {Count} pipelines successfully.", pipelines.Items.Count());
            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Message = "Pipelines retrieved successfully.",
                Result = pipelines,
                StatusCode = System.Net.HttpStatusCode.OK
            });
        }

        /// <summary>
        /// Retrieves a pipeline by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier (GUID) of the pipeline to retrieve.</param>
        /// <param name="pagination">Pagination and sorting options (optional).</param>
        /// <returns>The pipeline matching the specified ID.</returns>
        /// <response code="200">Pipeline retrieved successfully.</response>
        /// <response code="404">No pipeline found with the specified ID.</response>

        [HttpGet("get-by-id/{id:guid}")]
        public async Task<ActionResult<ApiResponse>> GetPipelineById(Guid id ,[FromQuery] PaginationDto pagination)
        {
            var pipeline = await mediator.Send(new GetPipelineByIdQuery(id));
            if (pipeline == null)
            {
                logger.LogWarning("No pipelines found.");
                return NotFound(new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Message = "No pipelines found."
                });
            }
            logger.LogInformation("Retrieved pipeline successfully.");
            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Message = "Pipeline retrieved successfully.",
                Result = pipeline,
                StatusCode = System.Net.HttpStatusCode.OK
            });
        }
        /// <summary>
        /// Retrieves all pipelines created by a specific user.
        /// </summary>
        /// <param name="uid">The unique identifier (GUID) of the user whose pipelines to retrieve.</param>
        /// <param name="pagination">Pagination and sorting options.</param>
        /// <returns>List of pipelines created by the specified user.</returns>
        /// <response code="200">Pipelines retrieved successfully.</response>
        /// <response code="404">No pipelines found for the specified user.</response>

        [HttpGet("get-by-uid/{uid}")]
        public async Task<ActionResult<ApiResponse>> GetPipelineByUid(Guid uid ,[FromQuery] PaginationDto pagination)
        {
            Expression<Func<Domain.Models.Pipeline, bool>> filter
                = pi => pi.UserId == uid;

            var pipeline = await mediator.Send(new GetPipelinesByQuery(filter));
            if (pipeline == null)
            {
                logger.LogWarning("No pipelines found.");
                return NotFound(new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Message = "No pipelines found."
                });
            }
            logger.LogInformation("Retrieved pipeline successfully.");
            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Message = "Pipeline retrieved successfully.",
                Result = pipeline,
                StatusCode = System.Net.HttpStatusCode.OK
            });
        }
        /// <summary>
        /// Retrieves pipelines filtered by public visibility (IsPublic).
        /// </summary>
        /// <param name="isPublic">Filter pipelines based on their visibility status.</param>
        /// <param name="pagination">Pagination and sorting options.</param>
        /// <returns>List of pipelines filtered by their IsPublic status.</returns>
        /// <response code="200">Pipelines retrieved successfully.</response>
        /// <response code="404">No pipelines found matching the visibility criteria.</response>

        [HttpGet("get-all-isPublic/{isPublic:bool}")]
        public async Task<ActionResult<ApiResponse>> GetPipelineIsPublic(bool isPublic ,[FromQuery] PaginationDto pagination)
        {

            Expression<Func<Domain.Models.Pipeline, bool>> filter
               = pi => pi.IsPublic == isPublic;

            var pipelines = await mediator.Send(new GetPipelinesByQuery(
                filter,
                pagination
                ));

            if (pipelines.Items == null || !pipelines.Items.Any())
            {
                logger.LogWarning("No pipelines found.");
                return NotFound(new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Message = "No pipelines found."
                });
            }

            logger.LogInformation("Retrieved {Count} pipelines successfully.", pipelines.Items.Count());
            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Message = "Pipelines retrieved successfully.",
                Result = pipelines,
                StatusCode = System.Net.HttpStatusCode.OK
            });
        }
        /// <summary>
        /// Creates a new pipeline.
        /// </summary>
        /// <param name="pipelineAdd">The pipeline data to be created.</param>
        /// <returns>The created pipeline details.</returns>
        /// <response code="200">Pipeline created successfully.</response>
        /// <response code="400">Invalid input or failed to create the pipeline.</response>
        [HttpPost("create")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ApiResponse>> CreatePipeline([FromBody] PipelineAddRequest pipelineAdd)
        {
            var response = await mediator.Send(new AddPipelineCommand(pipelineAdd));

            if(response == null)
            {
                logger.LogWarning("occour problem when saving pipeline");
                return BadRequest(new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Message = "occour problem when saving pipeline try later."
                });
            }
            logger.LogInformation("Piepline Saved Successfully.");
            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Message = "Piepline Saved Successfully.",
                Result = response,
                StatusCode = System.Net.HttpStatusCode.OK
            });
        }
        /// <summary>
        /// Updates an existing pipeline.
        /// </summary>
        /// <param name="pipelineUpdate">The updated pipeline details.</param>
        /// <returns>The updated pipeline information.</returns>
        /// <response code="200">Pipeline updated successfully.</response>
        /// <response code="400">Invalid input or failed to update the pipeline.</response>
        /// <response code="404">Pipeline not found.</response>
        [HttpPut("update")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ApiResponse>> UpdatePipeline([FromBody] PipelineUpdateRequest pipelineUpdate)
        {
            var response = await mediator.Send(new UpdatePipelineCommand(pipelineUpdate));

            if (response == null)
            {
                logger.LogWarning("occour problem when saving pipeline");
                return BadRequest(new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Message = "occour problem when saving pipeline try later."
                });
            }
            logger.LogInformation("Piepline Saved Successfully.");
            return Ok(new ApiResponse
            {
                IsSuccess = true,
                Message = "Piepline Saved Successfully.",
                Result = response,
                StatusCode = System.Net.HttpStatusCode.OK
            });
        }
        /// <summary>
        /// Deletes a pipeline by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the pipeline to delete.</param>
        /// <returns>Success or failure response.</returns>
        /// <response code="200">Pipeline deleted successfully.</response>
        /// <response code="404">Pipeline not found.</response>
        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ApiResponse>> DeletePipeline([FromQuery] Guid id)
        {
            var response = await mediator.Send(new DeletePipelineCommand(id));

            if (response == false)
            {
                logger.LogWarning("No pipelines found.");
                return NotFound(new ApiResponse
                {
                    IsSuccess = false,
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Message = "No pipelines found."
                });
            }

            logger.LogInformation("Pipeline Deleted Successfully.");

            return Ok();
        }
    }
}
