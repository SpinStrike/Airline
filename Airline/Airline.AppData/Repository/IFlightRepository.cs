using System;
using System.Linq;
using System.Collections.Generic;
using Airline.AppData.Model;

namespace Airline.AppData.Repository
{
    public interface IFlightRepository : IBaseRepository<Flight>
    {
        Flight Crete(string number,
            City fromCity,
            City toCity,
            DateTime departureDate,
            DateTime arrivalDate,
            FlightStatus status);

        Flight FindByNumber(string flightNumber);

        IQueryable<Flight> FindByFromCity(Guid idCityFrom, IQueryable<Flight> flights = null);

        IQueryable<Flight> FindByToCity(Guid idCity, IQueryable<Flight> flights = null);

        IQueryable<Flight> FindByDepartureDate(DateTime departureDate, IQueryable<Flight> flights = null);

        IQueryable<Flight> FindByArrivalDate(DateTime arrivalDate, IQueryable<Flight> flights = null);

        void AddAircrewMember(Flight flight, AircrewMember aircrewMember);

        void AddAircrewMembers(Flight flight, IEnumerable<AircrewMember> aircrewMember);

        void RemoveAircrewMember(Flight flight, AircrewMember aircrewMember);

        void SetStatus(Flight flight, FlightStatus status);

        void SetFromCity(Flight flight, City city);

        void SetToCity(Flight flight, City city);
    }
}
