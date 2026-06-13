using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WeatherWorkerService.Data;
using WeatherWorkerService.Mappers;
using WeatherWorkerService.Models;

namespace WeatherWorkerService;

public sealed class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public Worker(
        ILogger<Worker> logger,
        IServiceScopeFactory scopeFactory,
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        string[] cities = ["Warszawa", "Chełm", "Lublin"];

        string? apiKey = _configuration["OpenWeather:ApiKey"];

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            _logger.LogError("Brak klucza API OpenWeather w konfiguracji OpenWeather:ApiKey.");
            return;
        }

        HttpClient httpClient = _httpClientFactory.CreateClient();

        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Rozpoczęto pobieranie pogody: {Time}", DateTimeOffset.Now);

            foreach (string city in cities)
            {
                await FetchAndSaveWeatherAsync(
                    httpClient,
                    city,
                    apiKey,
                    stoppingToken
                );
            }

            _logger.LogInformation("Zakończono cykl pobierania pogody. Kolejny za 30 sekund.");

            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }

    private async Task FetchAndSaveWeatherAsync(
        HttpClient httpClient,
        string city,
        string apiKey,
        CancellationToken stoppingToken)
    {
        string encodedCity = Uri.EscapeDataString(city);

        string url =
            $"https://api.openweathermap.org/data/2.5/weather?q={encodedCity}&appid={apiKey}&units=metric&lang=pl";

        try
        {
            HttpResponseMessage response = await httpClient.GetAsync(url, stoppingToken);

            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync(stoppingToken);

            WeatherData? weatherData = JsonConvert.DeserializeObject<WeatherData>(content);

            if (weatherData is null)
            {
                _logger.LogWarning("Nie udało się zdeserializować pogody dla miasta: {City}", city);
                return;
            }

            Weather weather = WeatherMapper.MapToWeather(weatherData);

            using IServiceScope scope = _scopeFactory.CreateScope();

            ApplicationDbContext dbContext =
                scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            dbContext.WeatherRecords.Add(weather);

            await dbContext.SaveChangesAsync(stoppingToken);

            _logger.LogInformation(
                "Zapisano pogodę dla miasta {City}. Temperatura: {Temperature}°C, opis: {Description}",
                weather.City,
                weather.TemperatureC,
                weather.Description
            );
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(
                ex,
                "Błąd przy próbie pobrania pogody dla miasta {City}: {Message}",
                city,
                ex.Message
            );
        }
        catch (TaskCanceledException) when (stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker został zatrzymany.");
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(
                ex,
                "Błąd przy próbie zapisu pogody dla miasta {City} do bazy danych.",
                city
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Nieoczekiwany błąd podczas obsługi miasta {City}.",
                city
            );
        }
    }
}