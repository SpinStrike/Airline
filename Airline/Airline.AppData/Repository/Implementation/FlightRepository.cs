using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Airline.AppData.Model;

namespace Airline.AppData.Repository.Implementation
{
    public class FlightRepository : BaseRepository<Flight>, IFlightRepository
    {
        public FlightRepository(IDbRepository dbRepository)
            : base(dbRepository.GetDbInstance(), dbRepository.GetDbInstance().Flights)
        {
            _points = GetContext().Set<FlightPoint>();
        }

        public Flight Crete(string number, 
            City fromCity, 
            City toCity, 
            DateTime departureDate, 
            DateTime arrivalDate,
            FlightStatus status = FlightStatus.Preparing)
        {
            var flight = new Flight()
            {
                Number = number,
                From = fromCity,
                To = toCity,
                DepartureDate = departureDate,
                ArrivalDate = arrivalDate,
                Status = status
            };

            this.Add(flight);

            return flight;
        }

        public override IQueryable<Flight> GetAll()
        {
            var targetFlights = base.GetAll().Include(x => x.Points.Select(y => y.City));

            return targetFlights;
        }

        public override Flight FindById(Guid id)
        {
            var targetFlight = GetAll().Include(x => x.Aircrew.Select(y => y.Profession))
                .FirstOrDefault(x => x.Id.Equals(id));

            return targetFlight;
        }

        public Flight FindByNumber(string flightNumber)
        {
            var targetFlight = GetAll().FirstOrDefault(x => x.Number.ToUpper().Equals(flightNumber.ToUpper()));

            return targetFlight;
        }

        public IQueryable<Flight> FindByFromCity(Guid idCityFrom, IQueryable<Flight> flights = null)
        {
            var targetFlights = flights ?? GetAll();

            flights = targetFlights.Where(x => x.Points.Where(y => y.CityId == idCityFrom && y.Direction == Direction.From).Count() == 1);

            return flights;
        }

        public IQueryable<Flight> FindByToCity(Guid idCityTo, IQueryable<Flight> flights = null)
        {
            var targetFlights = flights ?? GetAll();

            flights = targetFlights.Where(x => x.Points.Where(y => y.CityId == idCityTo && y.Direction == Direction.To).Count() == 1);

            return flights;
        }

        public IQueryable<Flight> FindByDepartureDate(DateTime departureDate, IQueryable<Flight> flights = null)
        {
            var targetFlights = flights ?? GetAll();

            flights = targetFlights.Where(x => x.DepartureDate == departureDate);

            return flights;
        }

        public IQueryable<Flight> FindByArrivalDate(DateTime arrivalDate, IQueryable<Flight> flights = null)
        {
            var targetFlights = flights ?? GetAll();

            flights = targetFlights.Where(x => x.ArrivalDate == arrivalDate);

            return flights;
        }

        public void SetStatus(Flight flight, FlightStatus status)
        {
            flight.Status = status;
        }

        public void AddAircrewMember(Flight flight, AircrewMember aircrewMember)
        {
            flight.Aircrew.Add(aircrewMember);
        }

        public void AddAircrewMembers(Flight flight, IEnumerable<AircrewMember> aircrewMember)
        {
            foreach (var user in aircrewMember)
            {
                flight.Aircrew.Add(user);
            }
        }

        public void RemoveAircrewMember(Flight flight, AircrewMember aircrewMember)
        {
            flight.Aircrew.Remove(aircrewMember);
        }

        public void SetFromCity(Flight flight, City city)
        {
            var toDelete = _points.FirstOrDefault(x => x.FlightId == flight.Id && x.Direction == Direction.From);

            flight.From = city;

            if (toDelete != null)
            {
                _points.Remove(toDelete);
            } 
        }

        public void SetToCity(Flight flight, City city)
        {
            var toDelete = _points.FirstOrDefault(x => x.FlightId == flight.Id && x.Direction == Direction.To);

            flight.To = city;

            if (toDelete != null)
            {
                _points.Remove(toDelete);
            }
        }

        private IDbSet<FlightPoint> _points;
    }
}
