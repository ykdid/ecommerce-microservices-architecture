using System.Text;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;

var builder = WebApplication.CreateBuilder(args);

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:3001", "http://localhost:5173")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

builder.Services
    .AddOcelot()
    .AddPolly();

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddJsonFile("Configuration/ocelot.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

var jwtSettingsSection = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettingsSection["SecretKey"];

if (string.IsNullOrEmpty(secretKey))
{
    throw new Exception("JWT SecretKey is not configured!");
}

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSettingsSection["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwtSettingsSection["Audience"],
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });

var app = builder.Build();

// Use CORS before other middleware
app.UseCors("AllowFrontend");

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// app.UseEndpoints(endpoints => endpoints.MapControllers());
await app.UseOcelot();

app.Run();