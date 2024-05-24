using Document.Models;

namespace Document.Middleware;

public class AuthMiddleware
{
    private readonly RequestDelegate _next;
    
    public AuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public async Task Invoke(HttpContext context, ApplicationContext dbContext)
    {
        if (!context.Request.Cookies.ContainsKey("AuthUserId"))
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync("Авторизуйся");
        }

        //context.Request.Headers["AuthUserId"].ToString();
        var auth = context.Request.Cookies["AuthUserId"];
        /*if (auth == String.Empty)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync("Авторизуйся");
        }*/
        var user = await dbContext.Users.FindAsync(Convert.ToInt64(auth) );
        if (user == null)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync("Такого пользователя нет");
        }

        await _next.Invoke(context);


    }
}