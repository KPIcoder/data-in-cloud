using System.Net;
using System.Text.Json;
using DataInCloud.Api.Shared.ErrorBoundary;

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        var errorResponse = new ErrorResponse
        {
            StatusCode = (int)HttpStatusCode.InternalServerError,
            Message = "An unexpected error occurred. Please try again later."
        };

        var result = JsonSerializer.Serialize(errorResponse);
        response.StatusCode = (int)HttpStatusCode.InternalServerError;
        return response.WriteAsync(result);
    }
}

