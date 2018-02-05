using Airline.AppData.Model;
using System;
using System.Linq;

namespace Airline.AppData.Repository
{
    public interface IFlightRequestRepository : IBaseRepository<FlightRequest>
    {
        void Add(AppUser from,
            AppUser to,
            string message,
            DateTime date,
            AdminAnswerStatus status);

        IQueryable<FlightRequest> GetFlightRequestsByUser(Guid userId);
    }
}
