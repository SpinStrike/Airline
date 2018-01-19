using System;

namespace Airline.AppData.Model
{
    public class SendReceiveBroker : Entity
    {
        public Direction? Direction { get; set; }

        public string UserId { get; set; }
        public AppUser User { get; set; }

        public Guid FlightRequestId { get; set; }
        public FlightRequest FlightRequest { get; set; }
    }
}
