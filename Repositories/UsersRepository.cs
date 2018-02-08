
using WeatherData.Models;

namespace WeatherData.Repositories 
{
    public class UsersRepository : Repository<ApplicationUser>, IUsersRepository
    {

        public UsersRepository(DatabaseContext context) : base(context) { }

        public DatabaseContext DatabaseContext 
        {
            get { return Context as DatabaseContext; }
        }

        // implementation of custom repository methods
    }
}