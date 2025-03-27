using Net9WebAPI.Application.Abstract;

namespace Net9WebAPI.WebAPI.Pipelines;

public class ApiRequestPipeline(IServiceProvider serviceProvider)
{
    public async Task<IApiResult> Pipe<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IApiRequest
    {
        // Insert any pre request execution behavior here

        // Insert validation here

        var handler = serviceProvider.GetRequiredService<IApiRequestHandler<TRequest>>();

        IApiResult apiResult = await handler.Handle(request, cancellationToken);

        return apiResult;
    }
}