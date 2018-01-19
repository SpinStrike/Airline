using System;

namespace Airline.AppData.Model
{
    /// <summary>
    /// This class represent relation between flight and from/to city.
    /// </summary>
    public class FlightPoint
    {
        public Guid FlightId { get; set; }
        public Flight Flight { get; set; }

        public Guid CityId { get; set; }
        public City City { get; set; }

        public Direction? Direction { get; set; }
    }
}
