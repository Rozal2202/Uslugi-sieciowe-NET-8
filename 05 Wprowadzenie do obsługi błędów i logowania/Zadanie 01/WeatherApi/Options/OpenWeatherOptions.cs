namespace WeatherApi.Options;

public sealed class OpenWeatherOptions
{
    public const string SectionName = "OpenWeather";

    public string ApiKey { get; init; } = string.Empty;
}