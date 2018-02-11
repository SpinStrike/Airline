using System;
using System.Collections.Generic;
using Airline.AppLogic.Dto;
using Airline.AppData.Model;

namespace Airline.AppLogic.Service
{
    /// <summary>
    /// Represent functionality to work with users that are aircrew member. 
    /// </summary>
    public interface IAircrewMemberService : IBaseService<AircrewMemberDto>
    {
        /// <summary>
        /// Get set of aircrew members that second name is equal with required.
        /// </summary>
        /// <param name="secondName">Second name of aircrew member.</param>
        /// <returns></returns>
        ServiceResult<IEnumerable<AircrewMemberDto>> FindBySecondName(string secondName);

        /// <summary>
        /// Get set of aircrew members that profession identifier is equal with required.
        /// </summary>
        /// <param name="idProfession">Identifier of profession.</param>
        /// <returns></returns>
        ServiceResult<IEnumerable<AircrewMemberDto>> FindByProfession(Guid idProfession);

        /// <summary>
        /// Get set of aircrew members that identifier is contains in identifiers list.
        /// </summary>
        /// <param name="memebrsId">List of aircrew member identifiers</param>
        /// <returns></returns>
        ServiceResult<IEnumerable<AircrewMemberDto>> FindByIds(IEnumerable<Guid> memebrsId);

        /// <summary>
        /// Get set of aircrew members that current location (city) is equal with required.
        /// </summary>
        /// <param name="idCity">City Identifier</param>
        /// <param name="available">Select only aircrew members with status = "Available".</param>
        /// <returns></returns>
        ServiceResult<IEnumerable<AircrewMemberDto>> FindByCurrentPosition(Guid idCity, bool available);

        /// <summary>
        /// Set status for all aircrew member that identifier contain in identifier list.
        /// </summary>
        /// <param name="aircrewMemberIds">List of users identifiers.</param>
        /// <param name="status">Status of aircrew member.</param>
        /// <returns></returns>
        ServiceAnswer SetStatus(IEnumerable<Guid> aircrewMemberIds, AircrewMemberStatus status);

        /// <summary>
        /// Set aircrew member profession.
        /// </summary>
        /// <param name="aircrewMemberId">Aircrew member identifier.</param>
        /// <param name="idProfession">Profession identifier.</param>
        /// <returns></returns>
        ServiceAnswer SetProfession(Guid aircrewMemberId, Guid idProfession);

        /// <summary>
        /// Set aircrew member current location (city).
        /// </summary>
        /// <param name="aircrewMemberId">Aircrew member identifier.</param>
        /// <param name="idCity">City identifier.</param>
        /// <returns></returns>
        ServiceAnswer SetCurrentLocation(Guid aircrewMemberId, Guid idCity);

        /// <summary>
        /// Set status for aircrew member.
        /// </summary>
        /// <param name="aircrewMemberIds">List of users identifiers.</param>
        /// <param name="status">String value of status.</param>
        /// <returns></returns>
        ServiceAnswer SetNewStatus(Guid userId, string status);

        /// <summary>
        /// Check aircre member that he available to delete.
        /// </summary>
        /// <param name="id">Aircrew member identifier.</param>
        /// <returns></returns>
        ServiceResult<bool> IsAvailableToDelete(Guid id);
    }
}
