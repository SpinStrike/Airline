using System;

namespace Airline.AppData.Model
{
    /// <summary>
    /// Represent information about aircrew member user.
    /// </summary>
    public class AircrewMember : AppUser
    {
        public AircrewMemberStatus Status { get; set; } = AircrewMemberStatus.Available;

        public Guid?  ProfessionId { get; set; }
        public Profession Profession { get; set; }

        public Guid CityId { get; set; }
        public City CurrentLocation { get; set; }

        public Guid? FlightId { get; set; }
        public Flight Flight { get; set; }
    }

    public enum AircrewMemberStatus
    {
        Available,
        InFlight,
        Unavailable
    }
}
