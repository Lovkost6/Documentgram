using Document.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("WebApiDatabase")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyAllowSpecificOrigins",
        builder =>
        {
            builder
                .SetIsOriginAllowed(p => true)
                .WithOrigins("http://localhost:5173/", "http://*:5173")
                .AllowCredentials()
                .WithExposedHeaders("Authuserid") 
                .AllowAnyHeader()
                .AllowAnyMethod();

        });
});
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    //app.UseSwagger();
    //app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();
app.UseCors("MyAllowSpecificOrigins");
app.Run();
