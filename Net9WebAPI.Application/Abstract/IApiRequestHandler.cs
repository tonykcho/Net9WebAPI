namespace Net9WebAPI.Application.Abstract;

public interface IApiRequestHandler<in TRequest> where TRequest : IApiRequest
{
    public Task<IApiResult> Handle(TRequest request, CancellationToken cancellationToken);
}