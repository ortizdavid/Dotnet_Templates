using System.Net;
using TemplateApi.Common.Exceptions;

namespace TemplateApi.Common.Middlewares;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;

    public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
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
            _logger.LogError("Unhandled error ocurred: "+ ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        var response = exception switch
        {
            AppException ex => new { status = ex.StatusCode, message = ex.Message },
            Exception ex => new { status = (int)HttpStatusCode.InternalServerError, message = ex.Message },
            _ => new { status = (int)HttpStatusCode.InternalServerError, message = "An unexpected error occurred." }
        };
        context.Response.StatusCode = response.status;
        _logger.LogError(response.message);
        await context.Response.WriteAsJsonAsync(response);
    }
}