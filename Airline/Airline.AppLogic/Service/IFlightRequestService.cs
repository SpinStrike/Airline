using System;
using System.Collections.Generic;
using Airline.AppLogic.Dto;

namespace Airline.AppLogic.Service
{
    public interface IFlightRequestService : IBaseService<FlightRequestDto>
    {
        ServiceAnswer Create(string emailFrom, string emailTo, string message);

        ServiceResult<IEnumerable<FlightRequestDto>> GetUserFlightRequests(Guid userId);

        ServiceAnswer SetAnswerToRequest(Guid id, bool isCompleted);
    }
}
