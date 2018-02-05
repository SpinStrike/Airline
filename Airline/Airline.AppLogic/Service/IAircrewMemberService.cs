using System;
using System.Collections.Generic;
using Airline.AppLogic.Dto;
using Airline.AppData.Model;

namespace Airline.AppLogic.Service
{
    public interface IAircrewMemberService : IBaseService<AircrewMemberDto>
    {
        ServiceResult<IEnumerable<AircrewMemberDto>> FindBySecondName(string secondName);

        ServiceResult<IEnumerable<AircrewMemberDto>> FindByProfession(Guid idProfession);

        ServiceResult<IEnumerable<AircrewMemberDto>> FindByIds(IEnumerable<Guid> memebrsId);

        ServiceResult<IEnumerable<AircrewMemberDto>> FindByCurrentPosition(Guid idCity, bool available);

        ServiceAnswer SetStatus(IEnumerable<Guid> aircrewMemberIds, AircrewMemberStatus status);

        ServiceAnswer SetProfession(Guid aircrewMemberId, Guid idProfession);

        ServiceAnswer SetCurrentLocation(Guid aircrewMemberId, Guid idCity);

        ServiceAnswer SetNewStatus(Guid userId, string status);

        ServiceResult<bool> IsAvailableToDelete(Guid id);
    }
}
