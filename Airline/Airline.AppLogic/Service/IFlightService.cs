using System;
using System.Collections.Generic;
using Airline.AppData.Model;
using Airline.AppLogic.Dto;

namespace Airline.AppLogic.Service
{
    public interface IFlightService : IBaseService<FlightDto>
    {
        ServiceAnswer Crete(FlightDto flight);

        ServiceResult<IEnumerable<FlightDto>> FindByNumber(string flightNumber);

        ServiceResult<IEnumerable<FlightDto>> GetFilteredList(Guid? fromCityId,
            Guid? toCityId,
            DateTime? departDate,
            DateTime? arriveDate);

        ServiceResult<IEnumerable<string>> GetAvailableStatuses(FlightDto flight = null);

        FlightStatus Status(string status);

        ServiceAnswer UpdateFlight(FlightDto updatedFlight);

        ServiceAnswer SetStatus(Guid idFlight, string status);

        //ServiceResult<IEnumerable<FlightDto>> FindByFromToCities(Guid idCityFrom, Guid idCity);

        //ServiceResult<IEnumerable<FlightDto>> FindByFromCity(Guid idCityFrom);

        //ServiceResult<IEnumerable<FlightDto>> FindByToCity(Guid idCity);

        //ServiceResult<IEnumerable<FlightDto>> FindByDepartureDate(DateTime departureDate);

        //ServiceResult<IEnumerable<FlightDto>> FindByArrivalDate(DateTime arrivalDate);

        //ServiceAnswer AddAircrewMember(Guid idFlight, Guid idAircrewMember);

        //ServiceAnswer RemoveAircrewMember(Guid idFlight, Guid idAircrewMember);
    }
}
