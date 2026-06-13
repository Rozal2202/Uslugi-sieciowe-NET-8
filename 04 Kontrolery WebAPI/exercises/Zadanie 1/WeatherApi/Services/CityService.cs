using WeatherApi.Models;

namespace WeatherApi.Services;

public sealed class CityService
{
    private readonly object _lock = new();

    private readonly List<CityDto> _cities =
    [
        new CityDto(1, "Warszawa", "PL"),
        new CityDto(2, "Kraków", "PL"),
        new CityDto(3, "Gdańsk", "PL")
    ];

    private int _nextId = 4;

    public IReadOnlyList<CityDto> GetAll()
    {
        lock (_lock)
        {
            return _cities.ToList();
        }
    }

    public CityDto? GetById(int id)
    {
        lock (_lock)
        {
            return _cities.FirstOrDefault(city => city.Id == id);
        }
    }

    public CityDto Create(CreateCityRequest request)
    {
        lock (_lock)
        {
            CityDto city = new CityDto(
                Id: _nextId,
                Name: request.Name.Trim(),
                Country: request.Country.Trim().ToUpperInvariant()
            );

            _cities.Add(city);
            _nextId++;

            return city;
        }
    }

    public CityDto? Update(int id, UpdateCityRequest request)
    {
        lock (_lock)
        {
            int cityIndex = _cities.FindIndex(city => city.Id == id);

            if (cityIndex == -1)
            {
                return null;
            }

            CityDto updatedCity = new CityDto(
                Id: id,
                Name: request.Name.Trim(),
                Country: request.Country.Trim().ToUpperInvariant()
            );

            _cities[cityIndex] = updatedCity;

            return updatedCity;
        }
    }

    public bool Delete(int id)
    {
        lock (_lock)
        {
            CityDto? city = _cities.FirstOrDefault(city => city.Id == id);

            if (city is null)
            {
                return false;
            }

            _cities.Remove(city);

            return true;
        }
    }
}