using Airline.AppData.Model;

namespace Airline.AppData.Repository
{
    public interface IProfessionRepository : IBaseRepository<Profession>
    {
        void Create(string professionName);
    }
}
