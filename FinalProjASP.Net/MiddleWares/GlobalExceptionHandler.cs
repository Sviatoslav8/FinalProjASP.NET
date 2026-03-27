using System.Text.Json;
using FluentValidation;

namespace CryptoProj.API.Middlewares;

public class GlobalExceptionHandler : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (Exception ex)
        {
            var statusCode = ex switch
            {
                ArgumentException => StatusCodes.Status400BadRequest,
                ValidationException => StatusCodes.Status400BadRequest,
                _ => StatusCodes.Status500InternalServerError
            };
            
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            await context.Response.Body.FlushAsync();

            if (ex is ValidationException validationException)
            {
                var errorValidation = validationException.Errors.Select(x => (x.PropertyName, x.ErrorMessage)).ToArray()[0];
                var error = new Error(statusCode, errorValidation.PropertyName, errorValidation.ErrorMessage);
                var errorJson = JsonSerializer.Serialize(error);
                await context.Response.WriteAsync(errorJson);
            }
            else
            {
                var error = new Error(statusCode, ex.Message, ex.InnerException?.Message);
                var errorJson = JsonSerializer.Serialize(error);
                await context.Response.WriteAsync(errorJson);
            }
        }
    }

    record Error(int StatusCode, string Message, string? Detail = null);
}
