using InternshipTask.Api.Middleware;
using InternshipTask.Application.Extensions;
using InternshipTask.Domain.Extensions;
using InternshipTask.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();
builder.Services.AddHealthChecks();
        
builder.Services.AddApplicationLayer(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddProviders();

builder.Services.AddSwaggerGen();
builder.Services.AddLogging();

var app = builder.Build();

app.MapGet("/", () => "API Крестики нолики");

app.UseHealthChecks("/health");
app.MapHealthChecks("/health");

app.UseMiddleware<ExceptionMiddleware>();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();
app.UseHttpsRedirection();

app.MigrateUp();

app.Run();

public partial class Program { }