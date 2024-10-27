using System.Web.Http.ExceptionHandling;
using FormBuilder.Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FormBuilder.Common.Middleware;

internal sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> HandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var problemDetails = new ProblemDetails();
        problemDetails.Instance = httpContext.Request.Path;
        if (exception is BaseException e)
        {
            httpContext.Response.StatusCode = (int)e.StatusCode;
            problemDetails.Title = e.Message;
        }
        else 
        {
            problemDetails.Title = exception.Message;
        }
        _logger.LogError("{ProblemDetailsTitle}", problemDetails.Title);
        problemDetails.Status = httpContext.Response.StatusCode;
        await httpContext.Response
            .WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}