using System.Net.Mime;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using Wallet.CrossCutting.Configuration.ResourceCatalog;
using Wallet.CrossCutting.Exception;

namespace Wallet.Api.Exceptions;

public class GlobalExceptionHandler(
    ILogger<GlobalExceptionHandler> logger,
    ResourceCatalog resourceCatalog) : IExceptionHandler
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

    private ProblemDetails GenerateErrorResponse(Exception exception) =>
        exception switch
        {
            FluentValidation.ValidationException ex => HandleValidationException(ex),
            BaseException ex => HandleBaseException(ex),
            PostgresException => HandlePostgresException(),
            _ => HandleUnknownException(exception)
        };
    
    private ProblemDetails HandleValidationException(FluentValidation.ValidationException ex) =>
        new()
        {
            Title = "One or more validation errors occurred.",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Status = StatusCodes.Status400BadRequest,
            Extensions = { ["errors"] = ex.Errors.Select(e => resourceCatalog.GetMessage(e.ErrorMessage)).ToList() }
            
        };

    private ProblemDetails HandleBaseException(BaseException ex) =>
        new()
        {
            Title = "Application Error",
            Type = $"https://httpstatuses.io/{ex.StatusCode}",
            Status = ex.StatusCode,
            Detail = resourceCatalog.GetMessage(ex.Message)
        };

    private ProblemDetails HandleUnknownException(Exception ex) =>
        new()
        {
            Title = "Internal Server Error",
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
            Status = StatusCodes.Status500InternalServerError,
            Detail = resourceCatalog.GetMessage(ex.Message)
        };
    
    private ProblemDetails HandlePostgresException() =>
        new()
        {
            Title = "Internal Server Error",
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
            Status = StatusCodes.Status500InternalServerError,
            Detail = resourceCatalog.GetMessage("Postgres_Error")
        };
}