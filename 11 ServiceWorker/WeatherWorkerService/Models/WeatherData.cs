using Newtonsoft.Json;

namespace WeatherWorkerService.Models;

public sealed class WeatherData
{
    [JsonProperty("name")]
    public string City { get; set; } = string.Empty;

    [JsonProperty("sys")]
    public WeatherSystem System { get; set; } = new();

    [JsonProperty("main")]
    public WeatherMain Main { get; set; } = new();

    [JsonProperty("weather")]
    public List<WeatherDescription> Weather { get; set; } = [];

    [JsonProperty("wind")]
    public WeatherWind Wind { get; set; } = new();
}

public sealed class WeatherSystem
{
    [JsonProperty("country")]
    public string Country { get; set; } = string.Empty;
}

public sealed class WeatherMain
{
    [JsonProperty("temp")]
    public double Temperature { get; set; }

    [JsonProperty("feels_like")]
    public double FeelsLike { get; set; }

    [JsonProperty("humidity")]
    public int Humidity { get; set; }

    [JsonProperty("pressure")]
    public int Pressure { get; set; }
}

public sealed class WeatherDescription
{
    [JsonProperty("description")]
    public string Description { get; set; } = string.Empty;
}

public sealed class WeatherWind
{
    [JsonProperty("speed")]
    public double Speed { get; set; }
}