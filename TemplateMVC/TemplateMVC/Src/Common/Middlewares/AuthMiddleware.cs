namespace TemplateMVC.Common.Middlewares;

public class AuthMiddleware
{
    private readonly RequestDelegate _next;

    public AuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }        

    public async Task InvokeAsync(HttpContext context)
    {
        var currentPath = context.Request.Path.Value ?? "";

        if (currentPath == "/" || 
            currentPath == "/auth/login" || 
            currentPath.StartsWith("/auth/get-recover-link") || 
            currentPath.StartsWith("/auth/recover-password"))
        {
            await _next(context);
            return;
        }

        if (string.IsNullOrEmpty(context.Session.GetString("UserName")))
        {
            context.Response.Redirect("/auth/login");
            return;
        }

        await _next(context);
    }

}