using System;

namespace Airline.AppData.Model
{
    /// <summary>
    /// This class represent information about aircrew member user.
    /// </summary>
    public class AircrewMember : AppUser
    {
        public Guid  ProfessionId { get; set; }
        public Profession Profession { get; set; }

        public Guid FlightId { get; set; }
        public Flight Flight { get; set; }
    }
}
