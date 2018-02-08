using System;

namespace WeatherData.Repositories 
{
    public interface IUnitOfWork : IDisposable
	{
	
		IUsersRepository Users { get; }
        IAddressesRepository Addresses { get; }
        IWeatherDatasRepository WeatherDatas { get; }

		int Complete();

	}
}