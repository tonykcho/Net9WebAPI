namespace Net9WebAPI.Application.Abstract;

public class ApiContentResult<TResponse>(TResponse content) : IApiContentResult<TResponse>
{
    public TResponse GetContent()
    {
        return content;
    }
}

public class ValidationProblemApiResult<TResponse>(TResponse content) : IApiContentResult<TResponse>
{
    public TResponse GetContent()
    {
        return content;
    }
}

public sealed class InvalidRequestApiResult : IApiResult
{
}

public sealed class NoContentApiResult : IApiResult
{
}

public sealed class ResourceNotFoundApiResult : IApiResult
{
}

public sealed class ResourceAlreadyExistApiResult : IApiResult
{
}