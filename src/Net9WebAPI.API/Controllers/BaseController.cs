using Microsoft.AspNetCore.Mvc;
using Net9WebAPI.Application.Abstract;

namespace Net9WebAPI.API.Controllers;

public abstract class BaseController : ControllerBase
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult CreateErrorResponse(IApiResult result)
    {
        if (result is ValidationProblemApiResult<IDictionary<string, string[]>> validationProblemResult)
        {
            IDictionary<string, string[]> content = validationProblemResult.GetContent();
            var validationProblemDetails = new ValidationProblemDetails(content);

            return ValidationProblem(validationProblemDetails);
        }

        return result switch
        {
            InvalidRequestApiResult => BadRequest(),
            ResourceNotFoundApiResult => NotFound(),
            ResourceAlreadyExistApiResult => BadRequest(),
            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }
}