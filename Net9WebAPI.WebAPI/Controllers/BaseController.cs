using Microsoft.AspNetCore.Mvc;
using Net9WebAPI.Application.Abstract;

namespace Net9WebAPI.WebAPI.Controllers;

public abstract class BaseController : ControllerBase
{
    public IActionResult CreateResponse(IApiResult result)
    {
        if (result.GetType().IsGenericType && result.GetType().GetGenericTypeDefinition() == typeof(ApiContentResult<>))
        {
            return Ok(((dynamic)result).GetContent());    
        }

        if (result.GetType().IsGenericType && result.GetType().GetGenericTypeDefinition() == typeof(ValidationProblemApiResult<>))
        {
            var validationProblemDetails = new ValidationProblemDetails(((dynamic)result).GetContent());

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