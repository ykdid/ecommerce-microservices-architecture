using System.Text;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;

var builder = WebApplication.CreateBuilder(args);

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

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// app.UseEndpoints(endpoints => endpoints.MapControllers());
await app.UseOcelot();

app.Run();