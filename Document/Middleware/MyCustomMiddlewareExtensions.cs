namespace Document.Middleware;

public static class MyCustomMiddlewareExtensions
{
    public static IApplicationBuilder UseMyCustomMiddleware(this IApplicationBuilder app)
    {
        app.UseMiddleware<AuthMiddleware>();
        return app;
    }
}