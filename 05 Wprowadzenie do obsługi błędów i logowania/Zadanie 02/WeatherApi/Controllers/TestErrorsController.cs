using Microsoft.AspNetCore.Mvc;

namespace WeatherApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class TestErrorsController : ControllerBase
{
    private readonly ILogger<TestErrorsController> _logger;

    public TestErrorsController(ILogger<TestErrorsController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Get()
    {
        _logger.LogInformation("Wywołano endpoint testowy generujący wyjątek");

        throw new Exception("Testowy wyjątek");
    }
}