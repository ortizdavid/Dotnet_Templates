using System.Net;
using Serilog.Context;
using TemplateRabbitMQApi.Common.Exceptions;

namespace TemplateRabbitMQApi.Common.Middlewares;

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
            _ => new { status = (int)HttpStatusCode.InternalServerError, message = $"An unexpected error occurred: {exception.Message}" }
        };
        context.Response.StatusCode = response.status;

        var routeData = context.GetRouteData();
        var controller = routeData.Values["controller"]?.ToString() ?? "Unknown";
        var action = routeData.Values["action"]?.ToString() ?? "Unknown";
        
        using (LogContext.PushProperty("Controller", controller))
        using (LogContext.PushProperty("Action", action))
        using (LogContext.PushProperty("StatusCode", context.Response.StatusCode))
        {
            _logger.LogError(exception, response.message);
        }
        
        await context.Response.WriteAsJsonAsync(response);
    }
}