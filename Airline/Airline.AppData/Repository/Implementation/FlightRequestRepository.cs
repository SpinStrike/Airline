using System;
using System.Linq;
using System.Data.Entity;
using Airline.AppData.Model;

namespace Airline.AppData.Repository.Implementation
{
    public class FlightRequestRepository : BaseRepository<FlightRequest>, IFlightRequestRepository
    {
        public FlightRequestRepository(IDbRepository dbRepository)
            :base(dbRepository.GetDbInstance(), dbRepository.GetDbInstance().FlightRequests)
        {}

        public void Add(AppUser from,
            AppUser to,
            string message,
            DateTime date,
            AdminAnswerStatus status)
        {
            var request = new FlightRequest()
            {
                From = from,
                To = to,
                Message = message,
                IsReaded = false,
                Status = status,
                SendTime = date
            };

            this.Add(request);
        }

        public IQueryable<FlightRequest> GetFlightRequestsByUser(Guid userId)
        {
            return GetAll().Include(x => x.RequestBrokers.Select(y => y.User))
                .Where(x => x.RequestBrokers.Where(y => y.User.Id == userId && y.Direction == Direction.To).Count() == 1);
        }

        public override FlightRequest FindById(Guid id)
        {
            return GetAll().Include(x => x.RequestBrokers.Select(y => y.User ))
                .FirstOrDefault(x => x.Id == id);
        }
    }
}
