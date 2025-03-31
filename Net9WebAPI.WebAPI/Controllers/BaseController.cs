using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Net9WebAPI.Application.Abstract;
using Net9WebAPI.Domain.Abstract;

namespace Net9WebAPI.WebAPI.Controllers;

public abstract class BaseController : ControllerBase
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult CreateResponse(IApiResult result)
    {
        if (result.GetType().IsGenericType && result.GetType().GetGenericTypeDefinition() == typeof(ApiContentResult<>))
        {
            var content = result.GetContent();
            return Ok(content);    
        }

        if (result.GetType().IsGenericType && result.GetType().GetGenericTypeDefinition() == typeof(ValidationProblemApiResult<>))
        {
            ModelStateDictionary content = result.GetContent() as ModelStateDictionary ?? new ModelStateDictionary();
            var validationProblemDetails = new ValidationProblemDetails(content);

            return ValidationProblem(validationProblemDetails);
        }

        return result switch
        {
            InvalidRequestApiResult => BadRequest(),
            NoContentApiResult => NoContent(),
            ResourceNotFoundApiResult => NotFound(),
            ResourceAlreadyExistApiResult => BadRequest(),
            _ => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }
}