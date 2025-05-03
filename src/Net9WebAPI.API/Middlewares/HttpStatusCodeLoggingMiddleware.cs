namespace Net9WebAPI.API.Middlewares;

public class HttpStatusCodeLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<HttpStatusCodeLoggingMiddleware> _logger;

    public HttpStatusCodeLoggingMiddleware(RequestDelegate next, ILogger<HttpStatusCodeLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);

        int statusCode = context.Response.StatusCode;

        if (statusCode >= 200 && statusCode < 400)
        {
            _logger.LogInformation($"Success: Request {context.TraceIdentifier} with status code {statusCode}");
        }
        else if (statusCode >= 400 && statusCode < 500)
        {
            _logger.LogWarning($"Client error: Request {context.TraceIdentifier} with status code {statusCode}");
        }
        else if (statusCode >= 500)
        {
            _logger.LogError($"Server error: Request {context.TraceIdentifier} with status code {statusCode}");
        }
    }
}