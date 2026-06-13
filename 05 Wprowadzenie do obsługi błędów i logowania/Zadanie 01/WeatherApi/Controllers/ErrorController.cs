using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace WeatherApi.Controllers;

[ApiController]
public sealed class ErrorController : ControllerBase
{
    [Route("/api/error")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult HandleError()
    {
        IExceptionHandlerFeature? exceptionFeature =
            HttpContext.Features.Get<IExceptionHandlerFeature>();

        if (exceptionFeature is not null)
        {
            Console.WriteLine(exceptionFeature.Error);

            return Problem(
                detail: exceptionFeature.Error.Message,
                title: "Wystąpił błąd"
            );
        }

        return Problem();
    }
}