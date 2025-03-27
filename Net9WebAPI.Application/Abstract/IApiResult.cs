namespace Net9WebAPI.Application.Abstract;

public interface IApiResult
{
}

public interface IApiContentResult<TResponse> : IApiResult
{
    public TResponse GetContent();
}