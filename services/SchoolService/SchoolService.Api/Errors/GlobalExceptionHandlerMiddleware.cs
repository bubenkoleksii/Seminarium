﻿namespace SchoolService.Api.Errors;

public class GlobalExceptionHandlerMiddleware(RequestDelegate next)
{
    private static readonly JsonSerializerOptions s_serializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            await HandleExceptionAsync(context, e);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var (code, result) = exception switch
        {
            ValidationException validationException => HandleValidationException(validationException),
            _ => HandleUnknownException(exception)
        };

        context.Response.StatusCode = (int)code;
        context.Response.ContentType = "application/json";
        return context.Response.WriteAsync(result);
    }

    private static (HttpStatusCode, string) HandleValidationException(ValidationException validationException)
    {
        var problemDetailsList = validationException.Errors
            .Select(error => new ProblemDetails
            {
                Detail = error.ErrorMessage,
                Title = error.ErrorCode.ToSnakeCase(),
                Status = (int)HttpStatusCode.BadRequest,
                Type = ErrorTypes.Validation,
                Extensions =
                {
                    { "params", error.PropertyName.ToSnakeCase() },
                    { "attempted", error.AttemptedValue }
                }
            }).ToList();

        Log.Error(validationException, "An validation exception occurred.");

        return (HttpStatusCode.BadRequest, JsonSerializer.Serialize(problemDetailsList, s_serializerOptions));
    }

    private static (HttpStatusCode, string) HandleUnknownException(Exception exception)
    {
        var internalServerProblemDetails = new ProblemDetails
        {
            Detail = "An unexpected error occurred on the server.",
            Status = (int)HttpStatusCode.InternalServerError,
            Type = ErrorTypes.Unknown,
        };

        Log.Error(exception, "An unhandled exception occurred.");

        return (HttpStatusCode.InternalServerError, JsonSerializer.Serialize(internalServerProblemDetails, s_serializerOptions));
    }
}

public static class CustomExceptionHandlerMiddlewareExtension
{
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder builder)
        => builder.UseMiddleware<GlobalExceptionHandlerMiddleware>();
}
