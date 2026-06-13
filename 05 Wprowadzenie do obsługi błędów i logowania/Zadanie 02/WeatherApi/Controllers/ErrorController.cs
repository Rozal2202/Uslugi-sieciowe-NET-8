using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WeatherApi.Controllers;

[ApiController]
public sealed class ErrorController : ControllerBase
{
    private readonly ILogger<ErrorController> _logger;

    public ErrorController(ILogger<ErrorController> logger)
    {
        _logger = logger;
    }

    [Route("/api/error")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult HandleError()
    {
        IExceptionHandlerFeature? exceptionFeature =
            HttpContext.Features.Get<IExceptionHandlerFeature>();

        if (exceptionFeature is not null)
        {
            _logger.LogError(
                exceptionFeature.Error,
                "Wystąpił nieobsłużony błąd aplikacji"
            );

            return Problem(
                detail: exceptionFeature.Error.Message,
                title: "Wystąpił błąd"
            );
        }

        _logger.LogError("Wystąpił nieznany błąd aplikacji");

        return Problem();
    }
}