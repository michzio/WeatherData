
using WeatherData.Models;

namespace WeatherData.Repositories 
{ 

    public class AddressesRepository : Repository<Address>, IAddressesRepository
    {
        public AddressesRepository(DatabaseContext context)
			: base(context)
		{
		}

		public DatabaseContext DatabaseContext
		{
			get { return Context as DatabaseContext; }
		}
    }
}