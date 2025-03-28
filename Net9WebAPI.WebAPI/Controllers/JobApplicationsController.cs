using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Net9WebAPI.Application.ApiRequests.JobApplications;
using Net9WebAPI.WebAPI.Pipelines;

namespace Net9WebAPI.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JobApplicationsController(ApiRequestPipeline apiRequestPipeline) : BaseController
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetJobApplicationsAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var result = await apiRequestPipeline.Pipe(new GetJobApplicationsRequest(), cancellationToken);

        return CreateResponse(result);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetJobApplicationAsync([FromRoute] GetJobApplicationRequest request, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var result = await apiRequestPipeline.Pipe(request, cancellationToken);

        return CreateResponse(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateJobApplicationAsync([FromBody] CreateJobApplicationRequest request, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var result = await apiRequestPipeline.Pipe(request, cancellationToken);

        return CreateResponse(result);
    }
}