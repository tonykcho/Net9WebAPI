using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Net9WebAPI.Application.ApiRequests.JobApplications;
using Net9WebAPI.WebAPI.Pipelines;

namespace Net9WebAPI.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JobApplicationsController(ApiRequestPipeline apiRequestPipeline) : BaseController
{
    [HttpGet("{id}")]
    public async Task<IActionResult> GetJobApplicationAsync([FromRoute] GetJobApplicationRequest request, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var result = await apiRequestPipeline.Pipe(request, cancellationToken);

        return CreateResponse(result);
    }
}