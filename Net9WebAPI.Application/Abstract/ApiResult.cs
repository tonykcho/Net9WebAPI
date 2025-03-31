namespace Net9WebAPI.Application.Abstract;

public abstract class ApiResultBase : IApiResult
{
    public virtual object? GetContent()
    {
        return null;
    }
}

public class ApiContentResult<TResponse>(TResponse content) : ApiResultBase
{
    public override object? GetContent()
    {
        return content;
    }

    public TResponse GetTypedContent()
    {
        return content;
    }
}

public class ValidationProblemApiResult<TResponse>(TResponse content) : ApiResultBase
{
    public override object? GetContent()
    {
        return content;
    }
}

public sealed class InvalidRequestApiResult : ApiResultBase
{
}

public sealed class NoContentApiResult : ApiResultBase
{
}

public sealed class ResourceNotFoundApiResult : ApiResultBase
{
}

public sealed class ResourceAlreadyExistApiResult : ApiResultBase
{
}