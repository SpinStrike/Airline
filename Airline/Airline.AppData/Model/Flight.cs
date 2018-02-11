using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace Airline.AppData.Model
{
    /// <summary>
    /// Represent all information about concrate flight.
    /// </summary>
    public class Flight : Entity
    {
        public string Number { get; set; }

        public DateTime DepartureDate { get; set; }

        public DateTime ArrivalDate { get; set; }

        public FlightStatus Status { get; set; } = FlightStatus.Preparing;

        public List<FlightPoint> Points { get; set; }

        public List<AircrewMember> Aircrew { get; set; }

        public string Name
        {
            get { return $"{From.Name} - {To.Name}"; }
        }

        public Flight()
        {
            Points = new List<FlightPoint>();
            Aircrew = new List<AircrewMember>();
        }

        /// <summary>
        /// Property represent from what city airplane will arrive.
        /// </summary>
        public City From
        {
            get
            {
                return GetPoint(Direction.From).City;
            }
            set
            {
                SetPoint(Direction.From, value);
            }
        }

        /// <summary>
        /// Property represent to what city airplane will arrive.
        /// </summary>
        public City To
        {
            get
            {
                return GetPoint(Direction.To).City;
            }
            set
            {
                SetPoint(Direction.To, value);
            }
        }


        public override string ToString()
        {
            var resultString = new StringBuilder();

            resultString.Append($"Flight id: {Id}\r\n");
            resultString.Append("Flight target points (cities):\r\n");
            resultString.Append($"From: {From.Name}(id: {From.Id})\r\n");
            resultString.Append($"To: {To.Name}(id: {To.Id})\r\n");
            resultString.Append($"Departure date: {DepartureDate.ToString("dd-MM-yyyy")}, Arrival date: {ArrivalDate.ToString("dd-MM-yyyy")}\r\n");
            resultString.Append($"Flight status: {Status.ToString()}\r\n");
            resultString.Append("Aircrew:\r\n");
            foreach(var member in Aircrew)
            {
                resultString.Append($"{member.FirstName} {member.SecondName}(Id: {member.Id})\r\n");
            }

            return resultString.ToString();
        }

        private FlightPoint GetPoint(Direction status)
        {
            return Points.FirstOrDefault(x => x.Direction == status);
        }

        private void SetPoint(Direction status, City city)
        {
            Points.Remove(Points.FirstOrDefault(x => x.Direction == status));

            Points.Add(new FlightPoint()
            {
                Direction = status,
                City = null,
                CityId = city.Id
            });
        }
    }

    public enum FlightStatus
    {
        Preparing, 
        InAir,    
        Landed,    
        Cancelled, 
    }
}
