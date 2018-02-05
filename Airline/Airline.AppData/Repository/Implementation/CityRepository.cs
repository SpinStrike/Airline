using Airline.AppData.Model;

namespace Airline.AppData.Repository.Implementation
{
    public class CityRepository : BaseRepository<City>, ICityRepository
    {
        public CityRepository(IDbRepository dbRepository)
            :base(dbRepository.GetDbInstance(), dbRepository.GetDbInstance().Cities)
        {}

        public void Create(string CityName)
        {
            var city = new City()
            {
                Name = CityName
            };

            this.Add(city);
        }
    }
}
