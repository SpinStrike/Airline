using System;
using System.Collections.Generic;
using System.Linq;
using Airline.AppData.Repository;
using Airline.AppData.Model;
using Airline.AppLogic.Dto;
using Airline.AppLogic.Logging;

namespace Airline.AppLogic.Service.Implementation
{
    /// <summary>
    /// Implement functionality to work with users that are aircrew member. 
    /// </summary>
    public class AircrewMemberService : IAircrewMemberService
    {
        public AircrewMemberService(IAircrewMemberRepository aircrewMemberRepository,
            IProfessionRepository professionRepository,
            ICityRepository cityRepository,
            IServiceLogger logger)
        {
            _aircrewMemberRepository = aircrewMemberRepository;
            _professionRepository = professionRepository;
            _cityRepository = cityRepository;
            _logger = logger;
        }

        /// <summary>
        /// Find aircrew member by his identifier.
        /// </summary>
        /// <param name="id">User identifier</param>
        /// <returns>Service result that contain result(found aircrew member), success/failure execution and error list of method.</returns>
        public ServiceResult<AircrewMemberDto> FindById(Guid id)
        {
            var result = new ServiceResult<AircrewMemberDto>();

            _logger.Debug("Start find aircrew member by id method.");

            try
            {
                var targetUser = _aircrewMemberRepository.FindById(id);

                result.Result = targetUser.ToDto();
                result.Status = AnswerStatus.Success;

                if (targetUser != null)
                {
                    _logger.Info($"Aircre member (id: {id}) was found.");
                }
                else
                {
                    _logger.Warning($"Aircre member (id: {id}) was not found.");
                }
            }
            catch (Exception exc)
            {
                _logger.Error($"Exception occurred during thefinding of a aircrew member by id: {id}.\r\n Exception: {exc.ToString()}");

                result.Result = null;
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            _logger.Debug("Finish find aircrew member by id method.");

            return result;
        }

        /// <summary>
        /// Get set of aircrew members that profession identifier is equal with required.
        /// </summary>
        /// <param name="idProfession">Identifier of profession.</param>
        /// <returns>Service result that contain result(list of aircrew members), success/failure execution and error list of method.</returns>
        public ServiceResult<IEnumerable<AircrewMemberDto>> FindByProfession(Guid idProfession)
        {
            var result = new ServiceResult<IEnumerable<AircrewMemberDto>>();

            _logger.Debug("Start find by profession aircrew members method.");

            try
            {
                var targetProfession = _professionRepository.FindById(idProfession);
                if (targetProfession != null)
                {
                    var targetUsers = _aircrewMemberRepository.FindByProfession(targetProfession.Id)
                        .ToList()
                        .Select(x => x.ToDto());

                    result.Result = targetUsers;
                    result.Status = AnswerStatus.Success;

                    _logger.Info($"Was found {targetUsers.Count()} aircrew members.");
                }
                else
                {
                    result.Status = AnswerStatus.Failure;
                    result.Errors.Add("Finding error", "Required profession was not found.");

                    _logger.Warning($"Profession with id: {idProfession} was not found.");
                }
            }
            catch (Exception exc)
            {
                _logger.Error($"Exception occurred during the finding of a aircrew members by profession id:{idProfession}.\r\n Exception: {exc.ToString()}");

                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            _logger.Debug("Finish find by profession aircrew members method.");

            return result;
        }

        /// <summary>
        /// Get set of aircrew members that identifier is contains in identifiers list.
        /// </summary>
        /// <param name="memebrsId">List of aircrew member identifiers</param>
        /// <returns>Service result that contain result(list of aircrew members), success/failure execution and error list of method.</returns>
        public ServiceResult<IEnumerable<AircrewMemberDto>> FindByIds(IEnumerable<Guid> memebrsId)
        {
            var result = new ServiceResult<IEnumerable<AircrewMemberDto>>();

            _logger.Debug("Start find aircrew member by ids method.");

            try
            {
                var targetUsers = _aircrewMemberRepository.GetAll()
                    .Where(x => memebrsId.Contains(x.Id))
                    .ToList()
                    .Select(x => x.ToDto());

                    result.Result = targetUsers;
                    result.Status = AnswerStatus.Success;

                    _logger.Info($"Was found {targetUsers.Count()} aircrew members.");
            }
            catch (Exception exc)
            {
                _logger.Error($"Exception occurred during the finding of a aircrew members by ids:\r\n {memebrsId.GetStringsLits()}.\r\n Exception: {exc.ToString()}");

                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            _logger.Debug("Finish find aircrew member by ids method.");

            return result;
        }

        /// <summary>
        /// Get set of aircrew members that second name is equal with required.
        /// </summary>
        /// <param name="secondName">Second name of aircrew member.</param>
        /// <returns>Service result that contain result(found aircrew member), success/failure execution and error list of method.</returns>
        public ServiceResult<IEnumerable<AircrewMemberDto>> FindBySecondName(string secondName)
        {
            var result = new ServiceResult<IEnumerable<AircrewMemberDto>>();

            _logger.Debug("Start find aircrew members by second name.");

            if(secondName == null || secondName.Equals(string.Empty))
            {
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Finding error", "Parametr Second name is empty.");

                _logger.Warning("Parametr second name is empty.");
                _logger.Debug("Finish find aircrew members by second name method.");

                return result;
            }

            try
            {
                var targetUsers = _aircrewMemberRepository.FindBySecondName(secondName)
                    .ToList()
                    .Select(x => x.ToDto());

                result.Result = targetUsers;
                result.Status = AnswerStatus.Success;

                _logger.Info($"Was found {targetUsers.Count()} aircrew members.");
            }
            catch (Exception exc)
            {
                _logger.Error($"Exception occurred during the finding of a aircrew members by second name :{secondName}.\r\n Exception: {exc.ToString()}");

                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            _logger.Debug("Finish find aircrew members by second name method.");

            return result;
        }

        /// <summary>
        /// Get set of aircrew members that current location (city) is equal with required. Checks for the existence of a city.
        /// </summary>
        /// <param name="idCity">City Identifier</param>
        /// <param name="available">Select only aircrew members with status = "Available".</param>
        /// <returns>Service result that contain result(found aircrew members), success/failure execution and error list of method.</returns>
        public ServiceResult<IEnumerable<AircrewMemberDto>> FindByCurrentPosition(Guid idCity, bool available = false)
        {
            var result = new ServiceResult<IEnumerable<AircrewMemberDto>>();

            _logger.Debug("Start find aircrew members by current position method.");

            try
            {
                var targetCity = _cityRepository.FindById(idCity);
                if (targetCity != null)
                {
                    var targetUsers = _aircrewMemberRepository.FindByCurentPosition(idCity);
                    if(available)
                    {
                        targetUsers =  targetUsers.Where(x => x.Status == AircrewMemberStatus.Available);
                    }
 
                    result.Result = targetUsers.ToList().Select(x => x.ToDto());
                    result.Status = AnswerStatus.Success;

                    _logger.Info($"Was found {targetUsers.Count()} aircre members.");
                    _logger.Debug("Finish find aircrew members by current position method.");

                    return result;
                }

                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Finding error", "Required city was not found.");

                _logger.Warning($"Required city (id : {idCity}) was not found.");
            }
            catch (Exception exc)
            {
                _logger.Error($"Exception occurred during the finding of a aircrew members by current user location id :{idCity}.\r\n Exception: {exc.ToString()}");

                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            _logger.Debug("Finish find aircrew members by current position method.");

            return result;
        }

        /// <summary>
        /// Get set of all aircrew members.
        /// </summary>
        /// <returns>Service result that contain result(found aircrew members), success/failure execution and error list of method.</returns>
        public ServiceResult<IEnumerable<AircrewMemberDto>> GetAll()
        {
            var result = new ServiceResult<IEnumerable<AircrewMemberDto>>();

            _logger.Debug("Start get all aircrew members method.");

            try
            {
                var targetUsers = _aircrewMemberRepository.GetAll()
                    .ToList()
                    .Select(x => x.ToDto());

                result.Result = targetUsers;
                result.Status = AnswerStatus.Success;

                _logger.Info($"Was found {targetUsers.Count()} aircrew members.");
            }
            catch (Exception exc)
            {
                _logger.Error($"Exception occurred during the getting all of a aircrew members.\r\n Exception: {exc.ToString()}");

                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            _logger.Debug("Finish get all aircrew members method.");

            return result;
        }

        /// <summary>
        /// Set aircrew member profession. Checks for the existence of a profession.
        /// </summary>
        /// <param name="aircrewMemberId">Aircrew member identifier.</param>
        /// <param name="idProfession">Profession identifier.</param>
        /// <returns>Service answer that contain success/failure execution and error list of method.</returns>
        public ServiceAnswer SetProfession(Guid aircrewMemberId, Guid idProfession)
        {
            var result = new ServiceAnswer();

            _logger.Debug("Start set aircrew member profession method.");

            try
            {
                var targetProfession = _professionRepository.FindById(idProfession);
                var targetUser = _aircrewMemberRepository.FindById(aircrewMemberId);
                if (targetProfession != null)
                {
                    if (targetUser != null)
                    {
                        _aircrewMemberRepository.StartTransaction();

                        _aircrewMemberRepository.SetProfession(targetUser, targetProfession); 

                        _aircrewMemberRepository.Commit();

                        result.Status = AnswerStatus.Success;

                        _logger.Info($"To aircrew member id: {aircrewMemberId} have been set new profession '{targetProfession.Name}' that have id:{idProfession}.");
                        _logger.Debug("Finish set aircrew member profession method.");

                        return result;
                    }

                    result.Status = AnswerStatus.Failure;
                    result.Errors.Add("Finding error", "Required aircrew member was not found.");

                    _logger.Warning($"Required aircrew member (id: {idProfession}) was not found.");
                }
                else
                {
                    result.Status = AnswerStatus.Failure;
                    result.Errors.Add("Finding error", "Required profession was not found.");

                    _logger.Warning($"Required profession (id: {idProfession}) was not found.");
                }

            }
            catch (Exception exc)
            {
                _professionRepository.RollBack();

                _logger.Error($"Exception occurred during the setting new profession of an aircrew member\r\n Id aircrew member: {aircrewMemberId}, Id profession: {idProfession}.\r\n Exception: {exc.ToString()}.");

                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            _logger.Debug("Finish set aircrew member profession method.");

            return result;
        }

        /// <summary>
        /// Set aircrew member current location (city). Checks for the existence of a city.
        /// </summary>
        /// <param name="aircrewMemberId">Aircrew member identifier.</param>
        /// <param name="idCity">City identifier.</param>
        /// <returns>Service answer that contain success/failure execution and error list of method.</returns>
        public ServiceAnswer SetCurrentLocation(Guid aircrewMemberId, Guid idCity)
        {
            var result = new ServiceAnswer();

            _logger.Debug("Start set aircrew member current location method.");

            try
            {
                var targetCity = _cityRepository.FindById(idCity);
                var targetUser = _aircrewMemberRepository.FindById(aircrewMemberId);
                if (targetCity != null)
                {
                    if (targetUser != null)
                    {
                        _aircrewMemberRepository.StartTransaction();

                        _aircrewMemberRepository.SetCity(targetUser, targetCity);

                        result.Status = AnswerStatus.Success;

                        _aircrewMemberRepository.Commit();

                        _logger.Info($"To aircrew member id: {aircrewMemberId} have been set new current position '{targetCity.Name}' that have id: {idCity}.");
                        _logger.Debug("Finish set aircrew member current location method.");

                        return result;
                    }

                    result.Status = AnswerStatus.Failure;
                    result.Errors.Add("Finding error", "Required aircrew member was not found.");

                    _logger.Warning($"Required aircrew member (Id: {aircrewMemberId}) was not found.");
                }
                else
                {
                    result.Status = AnswerStatus.Failure;
                    result.Errors.Add("Finding error", "Required profession was not found.");

                    _logger.Warning($"Required city (Id: {idCity}) was not found.");
                }
            }
            catch (Exception exc)
            {
                _aircrewMemberRepository.RollBack();

                _logger.Error($"Exception occurred during the setting new current position of an aircrew member\r\n Id aircrew member: {aircrewMemberId}, Id city: {idCity}.\r\n Exception: {exc.ToString()}");

                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            _logger.Debug("Finish set aircrew member current location method.");

            return result;
        }

        /// <summary>
        /// Set new status to ser of aircrew members.
        /// </summary>
        /// <param name="aircrewMemberIds">Aircre members identifiers.</param>
        /// <param name="status">New status</param>
        /// <returns>Service answer that contain success/failure execution and error list of method.</returns>
        public ServiceAnswer SetStatus(IEnumerable<Guid> aircrewMemberIds, AircrewMemberStatus status)
        {
            var result = new ServiceAnswer();

            _logger.Debug("Start set aircrew members status method.");

            try
            {
                 _aircrewMemberRepository.SetStatus(aircrewMemberIds, status);
                 result.Status = AnswerStatus.Success;

                _logger.Info($"To group of aircrew members, ids:\r\n {aircrewMemberIds.GetStringsLits()} have been set new status: {status.ToString()}.");
            }
            catch (Exception exc)
            {
                _logger.Error($"Exception occurred during the setting new status of group aircrew members, ids:\r\n {aircrewMemberIds.GetStringsLits()}.\r\n Exception: {exc.ToString()}");

                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            _logger.Debug("Finish set aircrew members status method.");

            return result;
        }

        /// <summary>
        /// Delete aircrew member. It is not implemented.
        /// </summary>
        /// <param name="id">Aircrew member identifier.</param>
        /// <returns></returns>
        public ServiceAnswer Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Set new status to aircrew member.
        /// </summary>
        /// <param name="userId">Aircrew member identifier.</param>
        /// <param name="status">New satus.</param>
        /// <returns>Service answer that contain success/failure execution and error list of method.</returns>
        public ServiceAnswer SetNewStatus(Guid userId, string status)
        {
            var result = new ServiceAnswer();

            _logger.Debug("Start set aircrew member new status method.");

            try
            {
                var targetUser = _aircrewMemberRepository.FindById(userId);
                if(targetUser != null)
                {
                    var newStatus = GetEnumStatus(status, targetUser.Status);
                    if(newStatus != targetUser.Status)
                    {
                        _aircrewMemberRepository.StartTransaction();

                        targetUser.Status = newStatus;

                        _aircrewMemberRepository.Commit();
                    }
                    
                    result.Status = AnswerStatus.Success;

                    _logger.Info($"To aircrew member (id: {userId}) has been set new status: '{status}'.");
                    _logger.Debug("Finish set  aircrew member new status method.");

                    return result;
                }
                else
                {  
                    result.Status = AnswerStatus.Failure;
                    result.Errors.Add("Finding error", "Aircrew member was not found.");

                    _logger.Warning($"Aircrew member (id: {userId}) was not found.");
                }
            }
            catch (Exception exc)
            {
                _aircrewMemberRepository.RollBack();

                _logger.Error($"Exception occurred during the setting new status of an aircrew member.\r\n User id: {userId}, new status: {status} .\r\n Exception: {exc.ToString()}");

                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            _logger.Debug("Finish set  aircrew member new status method.");

            return result;
        }

        /// <summary>
        /// Check for the available for deleting aircrew member.
        /// </summary>
        /// <param name="id">Aircrew member identifier.</param>
        /// <returns>Service result that contain bool answer, success/failure execution and error list of method.</returns>
        public ServiceResult<bool> IsAvailableToDelete(Guid id)
        {
            var result = new ServiceResult<bool>();

            _logger.Debug("Start is available to delete aircre member method.");

            try
            {
                var available = _aircrewMemberRepository.GetAll().Where(x => x.Id == id && x.Status != AircrewMemberStatus.InFlight).Count() == 1;
                if (available)
                {
                    result.Result = available;
                    result.Status = AnswerStatus.Success;
            
                    _logger.Info($"Aircre member (Id: {id}) is available to delete.");
                    _logger.Debug("Finish is available to delete aircre member method.");

                    return result;
                }

                result.Result = false;
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Confirmation error", "Selected aircrew member can't be deleted because it is in flight.");

                _logger.Warning($"Aircre member (Id: {id}) is not available to delete now.");
            }
            catch (Exception exc)
            {
                _logger.Error($"Exception occurred during the finding of an aircrew member, id: {id}.\r\n Exception: {exc.ToString()}");

                result.Result = false;
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            _logger.Debug("Finish is available to delete aircre member method.");

            return result;
        }

        /// <summary>
        /// Convert string to enum status.
        /// </summary>
        /// <param name="status">Status to convert.</param>
        /// <param name="currentStatus">Current aircrew member status.</param>
        /// <returns>Aircrew member status as enum variable.</returns>
        private AircrewMemberStatus GetEnumStatus(string status, AircrewMemberStatus currentStatus)
        {
            switch(status)
            {
                case "Available":
                    return AircrewMemberStatus.Available;
                case "InFlight":
                    return AircrewMemberStatus.InFlight;
                case "Unavailable":
                    return AircrewMemberStatus.Unavailable;
                default:
                    return currentStatus;
            }
        }
        
        private IAircrewMemberRepository _aircrewMemberRepository;
        private IProfessionRepository _professionRepository;
        private ICityRepository _cityRepository;
        private IServiceLogger _logger;
    }
}
