using System.Net;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace TemplateMVC.Common.Middlewares;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;

    public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Response.ContentType = "text/html";
        var errorPath = Path.Combine("./Views", "Error/Error.html");
        if (!File.Exists(errorPath))
        {
            await context.Response.WriteAsync("<h1>Oops! Something went wrong.</h1>");
        }
        var htmlContent = await File.ReadAllTextAsync(errorPath, Encoding.UTF8);
        htmlContent = htmlContent.Replace("{{ErrorMessage}}", exception.Message);
        await context.Response.WriteAsync(htmlContent);
    }
}