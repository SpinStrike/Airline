using Airline.AppData.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Airline.AppLogic.Service
{
    public interface IDbService : IDisposable
    {
        AirlineDbContext GetDbInstance();    
    }
}
