using FluentValidation;
using FluentValidation.Results;
using Net9WebAPI.Application.Abstract;

namespace Net9WebAPI.API.Pipelines;

public class ApiRequestPipeline(IServiceProvider serviceProvider)
{
    public async Task<IApiResult> Pipe<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IApiRequest
    {
        // Insert any pre request execution behavior here
        if (request is null)
        {
            return new InvalidRequestApiResult();
        }

        // Fluent Validation
        var validator = serviceProvider.GetService<IValidator<TRequest>>();

        if (validator is not null)
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (validationResult.IsValid == false)
            {
                IDictionary<string, string[]> errors = validationResult.ToDictionary();

                return new ValidationProblemApiResult<IDictionary<string, string[]>>(errors);
            }
        }

        var handler = serviceProvider.GetRequiredService<IApiRequestHandler<TRequest>>();

        IApiResult apiResult = await handler.Handle(request, cancellationToken);

        // Insert any post request execution behavior here

        return apiResult;
    }
}