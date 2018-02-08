using WeatherData.Models;

namespace WeatherData.Repositories 
{
    public interface IWeatherDatasRepository : IRepository<WeatherData.Models.WeatherData>
    { 
        WeatherData.Models.WeatherData GetLatest(); 
    }
}