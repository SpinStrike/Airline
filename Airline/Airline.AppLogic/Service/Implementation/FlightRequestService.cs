using System;
using System.Collections.Generic;
using System.Linq;
using Airline.AppLogic.Dto;
using Airline.AppData.Repository;
using Airline.AppData.Model;

namespace Airline.AppLogic.Service.Implementation
{
    public class FlightRequestService : IFlightRequestService
    {
        public FlightRequestService(IFlightRequestRepository flightRequestRepository,
           IUserRepository userRepository)
        {
            _flightRequestRepository = flightRequestRepository;
            _userRepository = userRepository;
        }

        public ServiceAnswer Create(string emailFrom, string emailTo, string message)
        {
            var result = new ServiceAnswer();

            try
            {
                var users = _userRepository.GetAll().Where(x => x.Email.ToUpper() == emailFrom.ToUpper() || 
                    x.Email.ToUpper() == emailTo.ToUpper())
                    .ToList();

                if (users.Count() == 2)
                {
                    var fromUser = users.First(x => x.Email == emailFrom);
                    var toUser = users.First(x => x.Email == emailTo);
                    var Message = message;

                    _flightRequestRepository.StartTransaction();

                    _flightRequestRepository.Add(fromUser, toUser, Message, DateTime.Now, AdminAnswerStatus.Undefined);

                    _flightRequestRepository.Commit();

                    result.Status = AnswerStatus.Success;

                    return result;
                }

                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Finding error", "One of the users is not found.");
            }
            catch (Exception exc)
            {
                _flightRequestRepository.RollBack();
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            return result;
        }

        public ServiceAnswer Delete(Guid id)
        {
            var result = new ServiceAnswer();

            try
            {
                var request = _flightRequestRepository.FindById(id);

                if (request != null)
                {
                    _flightRequestRepository.StartTransaction();

                    _flightRequestRepository.Delete(request);

                    _flightRequestRepository.Commit();

                    result.Status = AnswerStatus.Success;

                    return result;
                }

                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Deleting error", "Flight request is not found.");
            }
            catch (Exception exc)
            {
                _flightRequestRepository.RollBack();
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            return result;
        }

        public ServiceResult<FlightRequestDto> FindById(Guid id)
        {
            var result = new ServiceResult<FlightRequestDto>();

            try
            {
                var targetRequest = _flightRequestRepository.FindById(id);

                if (targetRequest != null)
                {
                    result.Result = targetRequest.ToDto();
                    result.Status = AnswerStatus.Success;

                    return result;
                }

                result.Result = null;
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Finding by id error", "Flight request is not found.");
            }
            catch (Exception exc)
            {
                result.Result = null;
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            return result;
        }

        public ServiceResult<IEnumerable<FlightRequestDto>> GetAll()
        {
            var result = new ServiceResult<IEnumerable<FlightRequestDto>>();

            try
            {
                var requests = _flightRequestRepository.GetAll()
                .OrderByDescending(x => x.SendTime)
                .ToList()
                .Select(x => x.ToDto());

                result.Status = AnswerStatus.Success;
                result.Result = requests;

            }
            catch (Exception exc)
            {
                result.Result = new List<FlightRequestDto>(); ;
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            return result;
        }

        public ServiceResult<IEnumerable<FlightRequestDto>> GetUserFlightRequests(Guid userId)
        {
            var result = new ServiceResult<IEnumerable<FlightRequestDto>>();

            try
            {
                var requests = _flightRequestRepository.GetFlightRequestsByUser(userId)
                .OrderByDescending(x => x.SendTime)
                .ToList()
                .Select(x => x.ToDto());

                result.Status = AnswerStatus.Success;
                result.Result = requests;

            }
            catch (Exception exc)
            {
                result.Result = new List<FlightRequestDto>();
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            return result;
        }

        public ServiceAnswer SetAnswerToRequest(Guid id, bool isCompleted)
        {
            var result = new ServiceAnswer();

            try
            {
                var targetRequest = _flightRequestRepository.FindById(id);

                if (targetRequest != null)
                {
                    var from = targetRequest.To;
                    var to = targetRequest.From;
                    var sendTime = DateTime.Now.Date;
                    var status = isCompleted ? AdminAnswerStatus.Completed : AdminAnswerStatus.Rejected;
                    var message = targetRequest.Message;

                    _flightRequestRepository.StartTransaction();

                    _flightRequestRepository.Delete(targetRequest);

                    _flightRequestRepository.Add(from, to, message, sendTime, status);

                    result.Status = AnswerStatus.Success;

                    _flightRequestRepository.Commit();

                    return result;
                }

                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Finding by id error", "Flight request is not found.");
            }
            catch (Exception exc)
            {
                _flightRequestRepository.RollBack();
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            return result;
        }

        private IFlightRequestRepository _flightRequestRepository;
        private IUserRepository _userRepository;
    }
}
