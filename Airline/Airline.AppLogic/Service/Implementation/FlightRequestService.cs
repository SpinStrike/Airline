using System;
using System.Collections.Generic;
using System.Linq;
using Airline.AppLogic.Dto;
using Airline.AppLogic.Logging;
using Airline.AppData.Repository;
using Airline.AppData.Model;

namespace Airline.AppLogic.Service.Implementation
{
    /// <summary>
    /// Implement functionality to work with flights requests. 
    /// </summary>
    public class FlightRequestService : IFlightRequestService
    {
        public FlightRequestService(IFlightRequestRepository flightRequestRepository,
           IUserRepository userRepository,
           IServiceLogger logger)
        {
            _flightRequestRepository = flightRequestRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        /// <summary>
        /// Create flight request.
        /// </summary>
        /// <param name="emailFrom">Sender e-mail.</param>
        /// <param name="emailTo">Receiver e-mail</param>
        /// <param name="message">Request message.</param>
        /// <returns>Service answer that contain success/failure execution and error list of method.</returns>
        public ServiceAnswer Create(string emailFrom, string emailTo, string message)
        {
            var result = new ServiceAnswer();

            _logger.Debug("Start create fligh request method.");

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

                    _logger.Info($"Added new flight request\r\n From: {emailFrom}\r\n To: {emailTo}.");
                    _logger.Debug("Finish create fligh request method.");

                    return result;
                }

                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Finding error", "One of the users is not found.");

                _logger.Warning($"User with e-mail: {emailTo} was not found.");
            }
            catch (Exception exc)
            {
                _flightRequestRepository.RollBack();

                _logger.Error($"Exception occurred during the adition of a new flight request.\r\n Exception: {exc.ToString()}");

                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            _logger.Debug("Finish create fligh request method.");

            return result;
        }

        /// <summary>
        /// Delete flight request by identifier.
        /// </summary>
        /// <param name="id">Flight request identifier.</param>
        /// <returns>Service answer that contain success/failure execution and error list of method.</returns>
        public ServiceAnswer Delete(Guid id)
        {
            var result = new ServiceAnswer();

            _logger.Debug("Start delete request message method.");

            try
            {
                var request = _flightRequestRepository.FindById(id);

                if (request != null)
                {
                    _flightRequestRepository.StartTransaction();

                    _flightRequestRepository.Delete(request);

                    _flightRequestRepository.Commit();

                    result.Status = AnswerStatus.Success;

                    _logger.Info($"Flight request (id: {id}) has been deleted.");
                    _logger.Debug("Finish delete request message method.");

                    return result;
                }

                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Deleting error", "Flight request is not found.");

                _logger.Warning($"Request (id: {id}) was not found.");
            }
            catch (Exception exc)
            {
                _flightRequestRepository.RollBack();

                _logger.Error($"Exception occurred during the deleting of a flight request.\r\n Exception: {exc.ToString()}");

                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            _logger.Debug("Finish delete request message method.");

            return result;
        }

        /// <summary>
        /// Find flight request by identifier.
        /// </summary>
        /// <param name="id">Flight request identifier.</param>
        /// <returns>Service result that contain result(found flight request), success/failure execution and error list of method.</returns>
        public ServiceResult<FlightRequestDto> FindById(Guid id)
        {
            var result = new ServiceResult<FlightRequestDto>();

            _logger.Debug("Start find flight request by id method.");

            try
            {
                var targetRequest = _flightRequestRepository.FindById(id);

                if (targetRequest != null)
                {
                    result.Result = targetRequest.ToDto();
                    result.Status = AnswerStatus.Success;

                    _logger.Info($"Flight request (id: {id}) was found.");
                    _logger.Debug("Finish find flight request by id method.");

                    return result;
                }

                result.Result = null;
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Finding by id error", "Flight request is not found.");

                _logger.Warning($"Flight request (id: {id}) was not found.");
            }
            catch (Exception exc)
            {
                _logger.Error($"Exception occurred during the finding of a flight request, id: {id}.\r\n Exception: {exc.ToString()}");

                result.Result = null;
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            _logger.Debug("Finish find flight request by id method.");

            return result;
        }

        /// <summary>
        /// Get set of all flight requests.
        /// </summary>
        /// <returns>Service result that contain result(found flight requests), success/failure execution and error list of method.</returns>
        public ServiceResult<IEnumerable<FlightRequestDto>> GetAll()
        {
            var result = new ServiceResult<IEnumerable<FlightRequestDto>>();

            _logger.Debug("Start get all flight requests method.");

            try
            {
                var requests = _flightRequestRepository.GetAll()
                //.OrderByDescending(x => x.SendTime)
                .ToList()
                .Select(x => x.ToDto());

                result.Status = AnswerStatus.Success;
                result.Result = requests;

                _logger.Info($"Was found {requests.Count()} flight requests.");

            }
            catch (Exception exc)
            {
                _logger.Error($"Exception occurred during the getting of all flight requests.\r\n Exception: {exc.ToString()}");

                result.Result = new List<FlightRequestDto>(); ;
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            _logger.Debug("Finish get all flight requests method.");

            return result;
        }

        /// <summary>
        /// Get set of flights requests where user idendifier equal request receiver identifier.
        /// </summary>
        /// <param name="userId">Receiver identifier.</param>
        /// <returns>Service result that contain result(found flight requests), success/failure execution and error list of method.</returns>
        public ServiceResult<IEnumerable<FlightRequestDto>> GetUserFlightRequests(Guid userId)
        {
            var result = new ServiceResult<IEnumerable<FlightRequestDto>>();

            _logger.Debug("Start find request flights by user method.");

            try
            {
                var requests = _flightRequestRepository.GetFlightRequestsByUser(userId)
                .OrderByDescending(x => x.SendTime)
                .ToList()
                .Select(x => x.ToDto());

                result.Status = AnswerStatus.Success;
                result.Result = requests;

                _logger.Info($"Was found {requests.Count()} flight requests.");
            }
            catch (Exception exc)
            {
                _logger.Error($"Exception occurred during the finding of all concrete user (id: {userId}) flight requests.\r\n Exception: {exc.ToString()}");

                result.Result = new List<FlightRequestDto>();
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            _logger.Debug("Finish find request flights by user method.");

            return result;
        }

        /// <summary>
        /// Create answer to existing request with one of the statuses "Completed" or "Rejected". 
        /// </summary>
        /// <param name="id">Flight request identifier.</param>
        /// <param name="isCompleted">true is "Completed" and false is Rejected</param>
        /// <returns>Service answer that contain success/failure execution and error list of method.</returns>
        public ServiceAnswer SetAnswerToRequest(Guid id, bool isCompleted)
        {
            var result = new ServiceAnswer();

            _logger.Debug("Start set answer to request method.");

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

                    _logger.Info($"Added new flight request (answer) from: {from.Email}, to: {to.Email}");
                    _logger.Debug("Finish set answer to request method.");

                    return result;
                }

                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Finding by id error", "Flight request is not found.");

                _logger.Warning($"Flight request (id: {id}) was not found.");
            }
            catch (Exception exc)
            {
                _logger.Error($"Exception occurred during the creating of an answer to flight request (id: {id}) flight requests.\r\n Exception: {exc.ToString()}");

                _flightRequestRepository.RollBack();
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            _logger.Debug("Finish set answer to request method.");

            return result;
        }

        private IFlightRequestRepository _flightRequestRepository;
        private IUserRepository _userRepository;
        private IServiceLogger _logger;
    }
}
