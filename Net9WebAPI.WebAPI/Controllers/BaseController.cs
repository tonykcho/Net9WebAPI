using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Net9WebAPI.Application.Abstract;
using Net9WebAPI.Domain.Abstract;

namespace Net9WebAPI.WebAPI.Controllers;

public abstract class BaseController : ControllerBase
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult CreateErrorResponse(IApiResult result)
    {
        if (result is ValidationProblemApiResult<ModelStateDictionary> validationProblemResult)
        {
            ModelStateDictionary content = validationProblemResult.GetContent();
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