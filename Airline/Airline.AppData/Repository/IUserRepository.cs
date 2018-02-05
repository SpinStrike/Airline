using System.Linq;
using Airline.AppData.Model;

namespace Airline.AppData.Repository
{
    public interface IUserRepository
    {
        IQueryable<AppUser> GetAll();
    }
}
