using System.Web.Http.ExceptionHandling;
using FormBuilder.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FormBuilder.Common.Middleware;

internal sealed class NotFoundExceptionHandler : IExceptionHandler
{
    private readonly ILogger<NotFoundExceptionHandler> _logger;

    public NotFoundExceptionHandler(ILogger<NotFoundExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is not NotFoundException notFoundException)
        {
            return false;
        }

        var problemDetails = new ProblemDetails()
        {
            Instance = httpContext.Request.Path,
            Title = "Not Found",
            Detail = exception.Message,
        };
        httpContext.Response.StatusCode = (int)notFoundException.StatusCode;
        _logger.LogError("{ProblemDetailsTitle}", problemDetails.Title);
        problemDetails.Status = httpContext.Response.StatusCode;
        await httpContext.Response
            .WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}