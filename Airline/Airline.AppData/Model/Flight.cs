using System;
using System.Collections.Generic;
using System.Linq;

namespace Airline.AppData.Model
{
    /// <summary>
    /// This class represent all information about concrate flight.
    /// </summary>
    public class Flight : Entity
    {
        public string Number { get; set; }

        public DateTime DepartureDate { get; set; }

        public DateTime ArrivalDate { get; set; }

        public FlightStatus Status { get; set; }

        public List<FlightPoint> Points { get; set; }

        public List<AircrewMember> Aircrew { get; set; }

        public Flight()
        {
            Points = new List<FlightPoint>();
            Aircrew = new List<AircrewMember>();
        }

        /// <summary>
        /// Property represent from what city plane will arrive.
        /// </summary>
        public City From
        {
            get
            {
                return GetPoint(FlightPointStatus.From).City;
            }
            set
            {
                SetPoint(FlightPointStatus.From, value);
            }
        }

        /// <summary>
        /// Property represent to what city plane will arrive.
        /// </summary>
        public City To
        {
            get
            {
                return GetPoint(FlightPointStatus.To).City;
            }
            set
            {
                SetPoint(FlightPointStatus.To, value);
            }
        }

        public string Name
        {
            get
            {
                return $"{From.Name} - {To.Name}";
            }
        }

        private FlightPoint GetPoint(FlightPointStatus status)
        {
            return Points.FirstOrDefault(x => x.Status == status);
        }

        private void SetPoint(FlightPointStatus status, City city)
        {
            var fromPoint = GetPoint(status);

            fromPoint.City = city;
            fromPoint.CityId = city.Id;
        }
    }

    public enum FlightStatus
    {
        Boarding,  //идет посадка
        Departed,  //вылетел
        InAir,     //в воздухе
        Landed,    //приземлился
        Cancelled, //отменен
        Delayed,   //задержка
    }
}
