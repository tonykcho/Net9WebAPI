using System.Threading;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Net9WebAPI.Application.Abstract;
using Net9WebAPI.Application.ApiRequests.JobApplications;
using Net9WebAPI.Application.Dtos;
using Net9WebAPI.API.Pipelines;

namespace Net9WebAPI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JobApplicationsController(ApiRequestPipeline apiRequestPipeline) : BaseController
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IList<JobApplicationDto>))]
    public async Task<IActionResult> GetJobApplications(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var result = await apiRequestPipeline.Pipe(new GetJobApplicationsRequest(), cancellationToken);

        if (result is ApiContentResult<IList<JobApplicationDto>> contentResult)
        {
            IList<JobApplicationDto> jobApplications = contentResult.GetContent();
            return Ok(jobApplications);
        }

        return CreateErrorResponse(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JobApplicationDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetJobApplication(int id, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var request = new GetJobApplicationRequest()
        {
            Id = id
        };

        var result = await apiRequestPipeline.Pipe(request, cancellationToken);

        if (result is ApiContentResult<JobApplicationDto> contentResult)
        {
            JobApplicationDto jobApplicationDto = contentResult.GetContent();
            return Ok(jobApplicationDto);
        }

        return CreateErrorResponse(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(JobApplicationDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateJobApplication([FromBody] CreateJobApplicationRequest request, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var result = await apiRequestPipeline.Pipe(request, cancellationToken);

        if (result is ApiContentResult<JobApplicationDto> contentResult)
        {
            JobApplicationDto jobApplicationDto = contentResult.GetContent();
            return CreatedAtAction(nameof(GetJobApplication), new { id = jobApplicationDto.Id }, jobApplicationDto);
        }

        return CreateErrorResponse(result);
    }
}