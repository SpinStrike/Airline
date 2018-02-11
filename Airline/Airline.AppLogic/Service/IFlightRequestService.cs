using System;
using System.Collections.Generic;
using Airline.AppLogic.Dto;

namespace Airline.AppLogic.Service
{
    /// <summary>
    /// Represent functionality to work with flights requests. 
    /// </summary>
    public interface IFlightRequestService : IBaseService<FlightRequestDto>
    {
        /// <summary>
        /// Create new flight request.
        /// </summary>
        /// <param name="emailFrom">Sender e-mail.</param>
        /// <param name="emailTo">Receiver e-mail.</param>
        /// <param name="message">Request message.</param>
        /// <returns></returns>
        ServiceAnswer Create(string emailFrom, string emailTo, string message);

        /// <summary>
        /// Get set of flights requests where user idendifier equal request receiver identifier.
        /// </summary>
        /// <param name="userId">Receiver identifier.</param>
        /// <returns></returns>
        ServiceResult<IEnumerable<FlightRequestDto>> GetUserFlightRequests(Guid userId);

        /// <summary>
        /// Create answer to existing request with one of the statuses "Completed" or "Rejected". 
        /// </summary>
        /// <param name="id">Flight request identifier.</param>
        /// <param name="isCompleted">true is "Completed" and false is Rejected</param>
        /// <returns></returns>
        ServiceAnswer SetAnswerToRequest(Guid id, bool isCompleted);
    }
}
