using System;
using System.Collections.Generic;

namespace Airline.AppData.Model
{
    /// <summary>
    /// This class represent information about aircrew member user.
    /// </summary>
    public class AircrewMember : AppUser
    {
        public AircrewMemberStatus? Status { get; set; } = AircrewMemberStatus.Available;

        public Guid  ProfessionId { get; set; }
        public Profession Profession { get; set; }

        public Guid FlightId { get; set; }
        public Flight Flight { get; set; }

        public List<ConfirmationRequest> ConfirmationRequests { get; set; }
    }

    public enum AircrewMemberStatus
    {
        Available,
        InFlight,
        Unavailable
    }
}
