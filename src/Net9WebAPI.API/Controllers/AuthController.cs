using Microsoft.AspNetCore.Mvc;
using Net9WebAPI.Application.Abstract;
using Net9WebAPI.API.Pipelines;

namespace Net9WebAPI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(ApiRequestPipeline apiRequestPipeline) : BaseController
{
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login(LoginRequest request, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var result = await apiRequestPipeline.Pipe(request, cancellationToken);

        if (result is ApiContentResult<LoginResultDto> contentResult)
        {
            return Ok(contentResult.GetContent());
        }

        return CreateErrorResponse(result);
    }
}