using System.Linq;
using System.Data.Entity;
using Airline.AppData.Model;
using Airline.AppData.EF;

namespace Airline.AppData.Repository.Implementation
{
    public class UserRepository : IUserRepository
    {
        public UserRepository(IDbRepository dbRepository)
        {
            _dbContext = dbRepository.GetDbInstance();
        }
        
        public IQueryable<AppUser> GetAll()
        {
            return _dbContext.Users.Include(x => x.Roles)
                .OrderBy(x => x.SecondName)
                .ThenBy(x => x.FirstName);
        }

        private AirlineDbContext _dbContext;
    }
}
