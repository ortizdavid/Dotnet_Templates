using TemplateSimpleMVC.Controllers;

namespace TemplateSimpleMVC.Middlewares;

public class AuthMiddleware
{
    private readonly RequestDelegate _next;

    public AuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }        

    public async Task InvokeAsync(HttpContext context)
    {
        var skipPaths = new[]{"/", "/Register", "/Auth/Login"};
        if (skipPaths.Contains(context.Request.Path.Value))
        {
            await _next(context);
            return;
        }
        var userName = context.Session.GetString("UserName");
        if (string.IsNullOrEmpty(userName))
        {
            context.Response.Redirect("/Auth/Login");
            return;
        }
        await _next(context);
    }
}