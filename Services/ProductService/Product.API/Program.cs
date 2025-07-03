using Microsoft.OpenApi.Models;
using Product.Application;
using Product.Application.DependencyInjection;
using Product.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddProblemDetails();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Product API",
        Version = "v1",
        Description = "OpenAPI documentation for the Product Microservice"
    });
});

var app = builder.Build();

app.Services.ApplyMigrations();
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(opt =>
    {
        opt.SwaggerEndpoint("/swagger/v1/swagger.json", "Product API v1");
        opt.RoutePrefix = string.Empty;
    });
}

// app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();