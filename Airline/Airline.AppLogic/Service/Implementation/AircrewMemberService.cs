using System;
using System.Collections.Generic;
using System.Linq;
using Airline.AppData.Repository;
using Airline.AppLogic.Dto;
using Airline.AppData.Model;

namespace Airline.AppLogic.Service.Implementation
{
    public class AircrewMemberService : IAircrewMemberService
    {
        public AircrewMemberService(IAircrewMemberRepository aircrewMemberRepository,
            IProfessionRepository professionRepository,
            ICityRepository cityRepository)
        {
            _aircrewMemberRepository = aircrewMemberRepository;
            _professionRepository = professionRepository;
            _cityRepository = cityRepository;
        }

        public ServiceResult<AircrewMemberDto> FindById(Guid id)
        {
            var result = new ServiceResult<AircrewMemberDto>();

            try
            {
                var targetUser = _aircrewMemberRepository.FindById(id);

                result.Result = targetUser.ToDto();
                result.Status = AnswerStatus.Success;
            }
            catch (Exception exc)
            {
                result.Result = null;
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            return result;
        }

        public ServiceResult<IEnumerable<AircrewMemberDto>> FindByProfession(Guid idProfession)
        {
            var result = new ServiceResult<IEnumerable<AircrewMemberDto>>();

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
                }
                else
                {
                    result.Status = AnswerStatus.Failure;
                    result.Errors.Add("Finding error", "Required profession was not found.");
                }
            }
            catch (Exception exc)
            {
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            return result;
        }

        public ServiceResult<IEnumerable<AircrewMemberDto>> FindByIds(IEnumerable<Guid> memebrsId)
        {
            var result = new ServiceResult<IEnumerable<AircrewMemberDto>>();

            try
            {
                var targetUsers = _aircrewMemberRepository.GetAll()
                    .Where(x => memebrsId.Contains(x.Id))
                    .ToList()
                    .Select(x => x.ToDto());

                    result.Result = targetUsers;
                    result.Status = AnswerStatus.Success;
            }
            catch (Exception exc)
            {
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            return result;
        }

        public ServiceResult<IEnumerable<AircrewMemberDto>> FindBySecondName(string secondName)
        {
            var result = new ServiceResult<IEnumerable<AircrewMemberDto>>();

            if(secondName == null || secondName.Equals(string.Empty))
            {
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Finding error", "Parametr Second name is empty.");

                return result;
            }

            try
            {
                var targetUsers = _aircrewMemberRepository.FindBySecondName(secondName)
                    .ToList()
                    .Select(x => x.ToDto());

                result.Result = targetUsers;
                result.Status = AnswerStatus.Success;
            }
            catch (Exception exc)
            {
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            return result;
        }

        public ServiceResult<IEnumerable<AircrewMemberDto>> FindByCurrentPosition(Guid idCity, bool available = false)
        {
            var result = new ServiceResult<IEnumerable<AircrewMemberDto>>();

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

                    return result;
                }

                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Finding error", "Required profession was not found.");
            }
            catch (Exception exc)
            {
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            return result;
        }

        public ServiceResult<IEnumerable<AircrewMemberDto>> GetAll()
        {
            var result = new ServiceResult<IEnumerable<AircrewMemberDto>>();

            try
            {
                var targetUsers = _aircrewMemberRepository.GetAll()
                    .ToList()
                    .Select(x => x.ToDto());

                result.Result = targetUsers;
                result.Status = AnswerStatus.Success;
            }
            catch (Exception exc)
            {
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            return result;
        }

        public ServiceAnswer SetProfession(Guid aircrewMemberId, Guid idProfession)
        {
            var result = new ServiceAnswer();

            try
            {
                var targetProfession = _professionRepository.FindById(idProfession);
                var targetUser = _aircrewMemberRepository.FindById(aircrewMemberId);
                if (targetProfession != null && targetUser != null)
                {
                    //_aircrewMemberRepository.SetProfession(aircrewMemberId, targetProfession); 

                    _aircrewMemberRepository.StartTransaction();

                    targetUser.Profession = targetProfession;

                    _aircrewMemberRepository.Commit();

                    result.Status = AnswerStatus.Success;

                    return result;
                }

                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Finding error", "Required profession was not found.");

            }
            catch (Exception exc)
            {
                _professionRepository.RollBack();
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            return result;
        }

        public ServiceAnswer SetCurrentLocation(Guid aircrewMemberId, Guid idCity)
        {
            var result = new ServiceAnswer();

            try
            {
                var targetCity = _cityRepository.FindById(idCity);
                var targetUser = _aircrewMemberRepository.FindById(aircrewMemberId);
                if (targetCity != null && targetUser != null)
                {
                    _aircrewMemberRepository.StartTransaction();

                    //_aircrewMemberRepository.SetCity(aircrewMemberId, targetCity);

                    targetUser.CurrentLocation = targetCity;

                    result.Status = AnswerStatus.Success;

                    _aircrewMemberRepository.Commit();

                    return result;
                }
 
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Finding error", "Required profession was not found.");
            }
            catch (Exception exc)
            {
                _aircrewMemberRepository.RollBack();
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            return result;
        }

        public ServiceAnswer SetStatus(IEnumerable<Guid> aircrewMemberIds, AircrewMemberStatus status)
        {
            var result = new ServiceAnswer();

            try
            {
                 _aircrewMemberRepository.SetStatus(aircrewMemberIds, status);
                 result.Status = AnswerStatus.Success;
            }
            catch (Exception exc)
            {
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            return result;
        }

        public ServiceAnswer Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public ServiceAnswer SetNewStatus(Guid userId, string status)
        {
            var result = new ServiceAnswer();

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

                    return result;
                }

                //_aircrewMemberRepository.SetStatus(aircrewMemberIds, status);
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Finding error", "Flight is not found.");

            }
            catch (Exception exc)
            {
                _aircrewMemberRepository.RollBack();
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            return result;
        }

        public ServiceResult<bool> IsAvailableToDelete(Guid id)
        {
            var result = new ServiceResult<bool>();

            try
            {
                var available = _aircrewMemberRepository.GetAll().Where(x => x.Id == id && x.Status != AircrewMemberStatus.InFlight).Count() == 1;
                if (available)
                {
                    result.Result = available;
                    result.Status = AnswerStatus.Success;

                    return result;
                }

                result.Result = false;
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Confirmation error", "Selected aircrew member can't be deleted because it is in flight.");
            }
            catch (Exception exc)
            {
                result.Result = false;
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            return result;
        }

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
    }
}
