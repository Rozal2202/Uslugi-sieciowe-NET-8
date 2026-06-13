using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using WeatherApi.Models;
using WeatherApi.Options;

namespace WeatherApi.Services;

public sealed class OpenWeatherService
{
    private readonly HttpClient _httpClient;
    private readonly OpenWeatherOptions _options;

    public OpenWeatherService(
        HttpClient httpClient,
        IOptions<OpenWeatherOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
    }

    public async Task<WeatherForecastDto?> GetWeatherAsync(
        string city,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(_options.ApiKey))
        {
            throw new InvalidOperationException(
                "Brak klucza API OpenWeather. Ustaw OpenWeather:ApiKey w user-secrets."
            );
        }

        string encodedCity = Uri.EscapeDataString(city.Trim());
        string encodedApiKey = Uri.EscapeDataString(_options.ApiKey);

        string requestUrl =
            $"data/2.5/weather?q={encodedCity}&appid={encodedApiKey}&units=metric&lang=pl";

        using HttpResponseMessage response = await _httpClient.GetAsync(
            requestUrl,
            cancellationToken
        );

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();

        OpenWeatherApiResponse? apiResponse =
            await response.Content.ReadFromJsonAsync<OpenWeatherApiResponse>(
                cancellationToken: cancellationToken
            );

        if (apiResponse is null)
        {
            return null;
        }

        OpenWeatherCondition? condition = apiResponse.Weather.FirstOrDefault();

        return new WeatherForecastDto(
            City: apiResponse.CityName,
            Country: apiResponse.System.Country,
            TemperatureC: apiResponse.Main.Temperature,
            FeelsLikeC: apiResponse.Main.FeelsLike,
            Humidity: apiResponse.Main.Humidity,
            Pressure: apiResponse.Main.Pressure,
            WindSpeed: apiResponse.Wind.Speed,
            Description: condition?.Description ?? "Brak opisu"
        );
    }
}