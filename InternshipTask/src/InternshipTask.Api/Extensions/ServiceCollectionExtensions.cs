using FluentValidation;
using InternshipTask.Application.Validators;
using Microsoft.AspNetCore.Mvc;

namespace InternshipTask.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFluentValidation(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddValidatorsFromAssemblyContaining<CreateGameValidator>();

        services.Configure<ApiBehaviorOptions>(x =>
        {
            x.InvalidModelStateResponseFactory = context =>
            {
                var problemDetails = new ValidationProblemDetails(context.ModelState)
                {
                    Type = $"https://http.dog/{StatusCodes.Status400BadRequest}",
                    Title = "Validation error",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = "One or more validation errors occurred.",
                    Instance = context.HttpContext.Request.Path
                };
                return new BadRequestObjectResult(problemDetails);
            };
        });
        
        return services;
    }
}