
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WeatherData.Models;

namespace WeatherData.Repositories 
{
    public class WeatherDatasRepository : Repository<WeatherData.Models.WeatherData>, IWeatherDatasRepository
    {
        public WeatherDatasRepository(DatabaseContext context)
			: base(context)
		{
		}

		public WeatherData.Models.WeatherData GetLatest() { 
			var weatherData = DatabaseContext.WeatherDatas.OrderByDescending(wd => wd.DateTime).Include(wd => wd.Address).FirstOrDefault();
			return weatherData; 
		}

		public DatabaseContext DatabaseContext
		{
			get { return Context as DatabaseContext; }
		}
    }
}