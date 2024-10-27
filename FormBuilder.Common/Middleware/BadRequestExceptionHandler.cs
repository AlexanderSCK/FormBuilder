using FormBuilder.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Diagnostics;
namespace FormBuilder.Common.Middleware;

internal sealed class BadRequestExceptionHandler : IExceptionHandler
{
    private readonly ILogger<BadRequestExceptionHandler> _logger;

    public BadRequestExceptionHandler(ILogger<BadRequestExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not BadRequestException badRequestException)
        {
            return false;
        }
        var problemDetails = new ValidationProblemDetails(badRequestException.Errors)
        {
            Instance = httpContext.Request.Path,
            Title = "Bad Request",
            Detail = exception.Message
        };
        httpContext.Response.StatusCode = (int) badRequestException.StatusCode;
        _logger.LogError("{ProblemDetailsTitle}", problemDetails.Title);
        problemDetails.Status = httpContext.Response.StatusCode;
        await httpContext.Response
            .WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}