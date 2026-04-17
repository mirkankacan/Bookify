using Bookify.Api.Extensions;
using Bookify.Api.Middleware;
using Bookify.Application;
using Bookify.Infrastructure;
using Carter;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
                     .AddJsonFile("appsettings.json", optional: false)
                     .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
                     .AddEnvironmentVariables();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication()
                .AddInfrastructure(builder.Configuration);

builder.Services.AddTransient<GlobalExceptionMiddleware>();
builder.Services.AddTransient<RequestContextLoggingMiddleware>();

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddCarter();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
    app.SeedData();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseGlobalExceptionMiddleware();
app.UseRequestContextLoggingMiddleware();
app.UseHttpsRedirection();
app.MapCarter();
app.Run();