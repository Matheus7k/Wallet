using System.Net.Mime;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Wallet.CrossCutting.Exception;

namespace Wallet.Api.Exceptions;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var problemDetails = GenerateErrorResponse(exception);

        problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;
        
        logger.LogError("{ProblemDetailsTitle}", problemDetails.Title);

        httpContext.Response.ContentType = MediaTypeNames.Application.Json;
        
        httpContext.Response.StatusCode = (int) problemDetails.Status!;
        
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken).ConfigureAwait(false);
        
        return true;
    }

    private static ProblemDetails GenerateErrorResponse(Exception exception) =>
        exception switch
        {
            FluentValidation.ValidationException ex => HandleValidationException(ex),
            BaseException ex => HandleBaseException(ex),
            _ => HandleUnknownException(exception)
        };
    
    private static ProblemDetails HandleValidationException(FluentValidation.ValidationException ex) =>
        new()
        {
            Title = "One or more validation errors occurred.",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Status = StatusCodes.Status400BadRequest,
            Extensions = { ["errors"] = ex.Errors.Select(e => e.ErrorMessage).ToList() }
            
        };

    private static ProblemDetails HandleBaseException(BaseException ex) =>
        new()
        {
            Title = "Application Error",
            Type = $"https://httpstatuses.io/{ex.StatusCode}",
            Status = ex.StatusCode,
            Detail = ex.Message
        };

    private static ProblemDetails HandleUnknownException(Exception ex) =>
        new()
        {
            Title = "Internal Server Error",
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
            Status = StatusCodes.Status500InternalServerError,
            Detail = ex.Message
        };
}