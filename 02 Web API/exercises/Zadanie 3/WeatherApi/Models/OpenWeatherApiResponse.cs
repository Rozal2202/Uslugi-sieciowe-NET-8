using System.Text.Json.Serialization;

namespace WeatherApi.Models;

public sealed class OpenWeatherApiResponse
{
    [JsonPropertyName("name")]
    public string CityName { get; init; } = string.Empty;

    [JsonPropertyName("sys")]
    public OpenWeatherSystem System { get; init; } = new();

    [JsonPropertyName("main")]
    public OpenWeatherMain Main { get; init; } = new();

    [JsonPropertyName("weather")]
    public IReadOnlyList<OpenWeatherCondition> Weather { get; init; } = Array.Empty<OpenWeatherCondition>();

    [JsonPropertyName("wind")]
    public OpenWeatherWind Wind { get; init; } = new();
}

public sealed class OpenWeatherSystem
{
    [JsonPropertyName("country")]
    public string Country { get; init; } = string.Empty;
}

public sealed class OpenWeatherMain
{
    [JsonPropertyName("temp")]
    public double Temperature { get; init; }

    [JsonPropertyName("feels_like")]
    public double FeelsLike { get; init; }

    [JsonPropertyName("humidity")]
    public int Humidity { get; init; }

    [JsonPropertyName("pressure")]
    public int Pressure { get; init; }
}

public sealed class OpenWeatherCondition
{
    [JsonPropertyName("description")]
    public string Description { get; init; } = string.Empty;
}

public sealed class OpenWeatherWind
{
    [JsonPropertyName("speed")]
    public double Speed { get; init; }
}