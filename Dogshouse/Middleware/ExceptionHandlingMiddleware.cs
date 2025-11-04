using Dogshouse.Exceptions;
using System.Net;
using System.Text.Json;

namespace Dogshouse.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context); 
        }
        catch (FluentValidation.ValidationException ex)
        {
            _logger.LogWarning(ex, "Validation failed");
            var errors = ex.Errors.Select(e => e.ErrorMessage).ToList();

            await HandleExceptionAsync(context, HttpStatusCode.BadRequest, "Validation failed", errors);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Entity not found");
            await HandleExceptionAsync(context, HttpStatusCode.NotFound, ex.Message);
        }
        catch (DuplicateDogNameException ex)
        {
            _logger.LogWarning(ex, "Duplicate dog name");
            await HandleExceptionAsync(context, HttpStatusCode.Conflict, ex.Message);
        }
        catch (JsonException ex)
        {
            _logger.LogWarning(ex, "Invalid JSON in request body");
            await HandleExceptionAsync(context, HttpStatusCode.BadRequest, "Invalid JSON format", new List<string> { ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error occurred");
            await HandleExceptionAsync(context, HttpStatusCode.InternalServerError, "Внутрішня помилка сервера");
        }
    }

    private async Task HandleExceptionAsync(
        HttpContext context,
        HttpStatusCode statusCode,
        string message,
        IEnumerable<string>? errors = null)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            Status = (int)statusCode,
            Title = message,
            Errors = errors 
        };

        var jsonOptions = new JsonSerializerOptions { WriteIndented = true };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response, jsonOptions));
    }
}
