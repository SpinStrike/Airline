using System;

namespace Airline.AppData.Model
{
    public class ConfirmationRequest : Entity
    {
        public string AircrewMemberId { get; set; }
        public AircrewMember AircrewMember { get; set; }

        public Guid FlightId { get; set; }
        public Flight Flight { get; set; }
    }
}
