using Microsoft.AspNetCore.Mvc;
using WeatherApi.Models;
using WeatherApi.Services;

namespace WeatherApi.Controllers;

[ApiController]
[Route("api/cities")]
public sealed class CitiesController : ControllerBase
{
    private readonly CityService _cityService;
    private readonly OpenWeatherService _openWeatherService;
    private readonly ILogger<CitiesController> _logger;

    public CitiesController(
        CityService cityService,
        OpenWeatherService openWeatherService,
        ILogger<CitiesController> logger)
    {
        _cityService = cityService;
        _openWeatherService = openWeatherService;
        _logger = logger;
    }

    [HttpGet]
    public ActionResult<IReadOnlyList<CityDto>> GetAll()
    {
        _logger.LogInformation("Pobrano listę miast");

        IReadOnlyList<CityDto> cities = _cityService.GetAll();

        return Ok(cities);
    }
    
    [HttpGet("{id:int}")]
    public ActionResult<CityDto> GetById(int id)
    {
        _logger.LogInformation("Próba pobrania miasta o id: {CityId}", id);

        CityDto? city = _cityService.GetById(id);

        if (city is null)
        {
            _logger.LogWarning("Nie znaleziono miasta o id: {CityId}", id);

            return NotFound(new
            {
                message = $"Nie znaleziono miasta o id: {id}."
            });
        }

        _logger.LogInformation("Pobrano miasto o id: {CityId}", id);

        return Ok(city);
    }
    
    [HttpPost]
    public ActionResult<CityDto> Create(CreateCityRequest request)
    {
        _logger.LogInformation("Próba dodania miasta: {CityName}", request.Name);

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            _logger.LogWarning("Nie podano nazwy miasta podczas dodawania");

            return BadRequest(new
            {
                message = "Nazwa miasta jest wymagana."
            });
        }

        if (string.IsNullOrWhiteSpace(request.Country))
        {
            _logger.LogWarning("Nie podano kodu kraju podczas dodawania miasta: {CityName}", request.Name);

            return BadRequest(new
            {
                message = "Kod kraju jest wymagany."
            });
        }

        CityDto createdCity = _cityService.Create(request);

        _logger.LogInformation(
            "Dodano miasto o id: {CityId}, nazwa: {CityName}",
            createdCity.Id,
            createdCity.Name
        );

        return CreatedAtAction(
            nameof(GetById),
            new { id = createdCity.Id },
            createdCity
        );
    }
    
    [HttpPut("{id:int}")]
    public ActionResult<CityDto> Update(int id, UpdateCityRequest request)
    {
        _logger.LogInformation("Próba aktualizacji miasta o id: {CityId}", id);

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            _logger.LogWarning("Nie podano nazwy miasta podczas aktualizacji miasta o id: {CityId}", id);

            return BadRequest(new
            {
                message = "Nazwa miasta jest wymagana."
            });
        }

        if (string.IsNullOrWhiteSpace(request.Country))
        {
            _logger.LogWarning("Nie podano kodu kraju podczas aktualizacji miasta o id: {CityId}", id);

            return BadRequest(new
            {
                message = "Kod kraju jest wymagany."
            });
        }

        CityDto? updatedCity = _cityService.Update(id, request);

        if (updatedCity is null)
        {
            _logger.LogWarning("Nie znaleziono miasta do aktualizacji o id: {CityId}", id);

            return NotFound(new
            {
                message = $"Nie znaleziono miasta o id: {id}."
            });
        }

        _logger.LogInformation(
            "Zaktualizowano miasto o id: {CityId}, nowa nazwa: {CityName}",
            updatedCity.Id,
            updatedCity.Name
        );

        return Ok(updatedCity);
    }
    
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        _logger.LogInformation("Próba usunięcia miasta o id: {CityId}", id);

        bool deleted = _cityService.Delete(id);

        if (!deleted)
        {
            _logger.LogWarning("Nie znaleziono miasta do usunięcia o id: {CityId}", id);

            return NotFound(new
            {
                message = $"Nie znaleziono miasta o id: {id}."
            });
        }

        _logger.LogInformation("Usunięto miasto o id: {CityId}", id);

        return NoContent();
    }
    
    [HttpGet("{id:int}/weather")]
    public async Task<ActionResult<WeatherForecastDto>> GetWeatherByCityId(
        int id,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Próba pobrania prognozy pogody dla miasta o id: {CityId}", id);

        CityDto? city = _cityService.GetById(id);

        if (city is null)
        {
            _logger.LogWarning("Nie znaleziono miasta do pobrania pogody o id: {CityId}", id);

            return NotFound(new
            {
                message = $"Nie znaleziono miasta o id: {id}."
            });
        }

        WeatherForecastDto? weather = await _openWeatherService.GetWeatherAsync(
            city.Name,
            cancellationToken
        );

        if (weather is null)
        {
            _logger.LogWarning("Nie znaleziono pogody dla miasta: {CityName}", city.Name);

            return NotFound(new
            {
                message = $"Nie znaleziono prognozy pogody dla miasta: {city.Name}."
            });
        }

        _logger.LogInformation("Pobrano pogodę dla miasta: {CityName}", city.Name);

        return Ok(weather);
    }
}