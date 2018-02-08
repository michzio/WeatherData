
using WeatherData.Models;

namespace WeatherData.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext Context; 

        public UnitOfWork(DatabaseContext context) {
            Context = context; 
            Users = new UsersRepository(context); 
            Addresses = new AddressesRepository(context);
            WeatherDatas = new WeatherDatasRepository(context); 
        }

        public IUsersRepository Users { get; private set; }
        public IAddressesRepository Addresses { get; private set; }
        public IWeatherDatasRepository WeatherDatas { get; private set; }

        public int Complete()
		{
			return Context.SaveChanges();
		}
		public void Dispose()
		{
			Context.Dispose();
		}
    }
}