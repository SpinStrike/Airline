using System.Collections.Generic;

namespace Airline.AppData.Model
{
    /// <summary>
    /// This class represent city as entity in application.
    /// </summary>
    public class City : Entity
    {
        public string Name { get; set; }

        public List<FlightPoint> Points { get; set; }

        public List<AppUser> Users { get; set; }
    }
}
