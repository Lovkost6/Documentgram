using Document.Middleware;
using Document.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("WebApiDatabase")));



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder
                .SetIsOriginAllowed(p => true)
                .WithOrigins("https://localhost:5173")
                .AllowCredentials()
                .WithExposedHeaders("Authuserid") 
                .AllowAnyHeader()
                .AllowAnyMethod();

        });
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.HttpOnly = false;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        options.ExpireTimeSpan = TimeSpan.FromHours(20);
        options.SlidingExpiration = true;
        options.Cookie.SameSite = SameSiteMode.None;
        options.LoginPath = "/v1/auth/Notauthorize";
        options.AccessDeniedPath = "/v1/auth/Forbidden";
        options.Cookie.Name = "userdata";
    });

var app = builder.Build();

app.UseCors("AllowSpecificOrigin");

/*app.UseWhen(context => context.Request.Path.ToString().StartsWith("/v1/auth") == false,
    appBuilder =>
    {
        appBuilder.UseMyCustomMiddleware();
    });*/

if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
