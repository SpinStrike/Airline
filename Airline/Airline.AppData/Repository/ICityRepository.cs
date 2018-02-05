using System;
using Airline.AppData.Model;

namespace Airline.AppData.Repository
{
    public interface ICityRepository : IBaseRepository<City> 
    {
        void Create(string CityName);
    }
}
