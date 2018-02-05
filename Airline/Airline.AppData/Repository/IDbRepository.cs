using System;
using Airline.AppData.EF;

namespace Airline.AppData.Repository
{
    public interface IDbRepository : IDisposable
    {
        AirlineDbContext GetDbInstance();
    }
}
