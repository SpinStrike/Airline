using System;
using System.Collections.Generic;
using Airline.AppData.Model;
using Airline.AppLogic.Dto;

namespace Airline.AppLogic.Service
{
    /// <summary>
    /// Represent functionality to work with flights. 
    /// </summary>
    public interface IFlightService : IBaseService<FlightDto>
    {
        /// <summary>
        /// Create new flight.
        /// </summary>
        /// <param name="flight">New flight data</param>
        /// <returns></returns>
        ServiceAnswer Crete(FlightDto flight);

        /// <summary>
        /// Find flight by flight number.
        /// </summary>
        /// <param name="flightNumber">Flight number</param>
        /// <returns></returns>
        ServiceResult<IEnumerable<FlightDto>> FindByNumber(string flightNumber);

        /// <summary>
        /// Get filtered list of flights.
        /// </summary>
        /// <param name="fromCityId">From city identifier.</param>
        /// <param name="toCityId">To city identifier.</param>
        /// <param name="departDate">Departure date.</param>
        /// <param name="arriveDate">Arrival date.</param>
        /// <returns></returns>
        ServiceResult<IEnumerable<FlightDto>> GetFilteredList(Guid? fromCityId,
            Guid? toCityId,
            DateTime? departDate,
            DateTime? arriveDate);

        /// <summary>
        /// Get available flight statuses. 
        /// </summary>
        /// <param name="flight">Flight data</param>
        /// <returns></returns>
        ServiceResult<IEnumerable<string>> GetAvailableStatuses(FlightDto flight = null);

        /// <summary>
        /// Convert from string status to enum flight status variable.
        /// </summary>
        /// <param name="status">Status as string.</param>
        /// <returns></returns>
        FlightStatus Status(string status);

        /// <summary>
        /// Update flight data.
        /// </summary>
        /// <param name="updatedFlight">New flight data.</param>
        /// <returns></returns>
        ServiceAnswer UpdateFlight(FlightDto updatedFlight);

        /// <summary>
        /// Set new status to flight.
        /// </summary>
        /// <param name="idFlight">Flight identifier.</param>
        /// <param name="status">Status as string.</param>
        /// <returns></returns>
        ServiceAnswer SetStatus(Guid idFlight, string status);
    }
}
