using System;
using System.Collections.Generic;
using System.Linq;
using Airline.AppData.Repository;
using Airline.AppLogic.Dto;
using Airline.AppLogic.Logging;

namespace Airline.AppLogic.Service.Implementation
{
    /// <summary>
    /// Implementation of  functionality to finde user by diffrent parametrs. 
    /// </summary>
    public class UserSearchService  : IUserSearchService
    {
        public UserSearchService(IUserRepository userRepository,
            IServiceLogger logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        /// <summary>
        /// Finde users by second name.
        /// </summary>
        /// <param name="secondName">User secon name.</param>
        /// <returns>Service result that contain result(found users), success/failure execution and error list of method.</returns>
        public ServiceResult<IEnumerable<UserDto>> FindeBySecondName(string secondName)
        {
            var result = new ServiceResult<IEnumerable<UserDto>>();

            _logger.Debug("Start find users by second name method.");

            try
            {
                result.Result = _userRepository.GetAll().Where(x => x.SecondName == secondName)
                .ToList()
                .Select(x => x.ToDto(true));

                result.Status = AnswerStatus.Success;

                _logger.Info($"Was found {result.Result.Count()} users.");
            }
            catch (Exception exc)
            {
                _logger.Error($"Exception occurred during the finding  users by second name.\r\n Exception: {exc.ToString()}");

                result.Result = new List<UserDto>();
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            _logger.Debug("Finish find users by second name method.");

            return result;
        }

        /// <summary>
        /// Finde by e-mail.
        /// </summary>
        /// <param name="email">User e-mail.</param>
        /// <returns>Service result that contain result(found users), success/failure execution and error list of method.</returns>
        public ServiceResult<IEnumerable<UserDto>> FindeByEmail(string email)
        {
            var result = new ServiceResult<IEnumerable<UserDto>>();

            _logger.Debug("Start find user by e-mail method.");

            try
            {
                result.Result =  _userRepository.GetAll().Where(x => x.Email == email)
                .ToList()
                .Select(x => x.ToDto(true));

                result.Status = AnswerStatus.Success;

                _logger.Info($"Was found {result.Result.Count()} users.");
            }
            catch (Exception exc)
            {
                _logger.Error($"Exception occurred during the finding users by e-mail.\r\n Exception: {exc.ToString()}");

                result.Result = new List<UserDto>();
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            _logger.Debug("Finish find user by e-mail method.");

            return result;
        }

        /// <summary>
        /// Finde by role.
        /// </summary>
        /// <param name="email">User role.</param>
        /// <returns>Service result that contain result(found users), success/failure execution and error list of method.</returns>
        public ServiceResult<IEnumerable<UserDto>> FindeByRole(string role)
        {
            var result = new ServiceResult<IEnumerable<UserDto>>();

            _logger.Debug("Start find users by role method.");

            try
            {
                result.Result = _userRepository.GetAll()
                    .ToList()
                    .Where(x => x.Roles.First().Name == role)
                    .Select(x => x.ToDto(true));

                result.Status = AnswerStatus.Success;

                _logger.Info($"Was found {result.Result.Count()} users.");
            }
            catch (Exception exc)
            {
                _logger.Error($"Exception occurred during the finding users by role.\r\n Exception: {exc.ToString()}");

                result.Result = new List<UserDto>();
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            _logger.Debug("Finish find users by role method.");

            return result;
        }

        /// <summary>
        /// Get set of all users.
        /// </summary>
        /// <returns>Service result that contain result(found users), success/failure execution and error list of method.</returns>
        public ServiceResult<IEnumerable<UserDto>> GetAllUsers()
        {
            var result = new ServiceResult<IEnumerable<UserDto>>();

            _logger.Debug("Start get all users method.");

            try
            {
                result.Result = _userRepository.GetAll().ToList()
                    .Select(x => x.ToDto(true));

                result.Status = AnswerStatus.Success;

                _logger.Info($"Was found {result.Result.Count()} users.");
            }
            catch (Exception exc)
            {
                _logger.Error($"Exception occurred during the getting all users.\r\n Exception: {exc.ToString()}");

                result.Result = new List<UserDto>();
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            _logger.Debug("Finish get all users method.");

            return result;
        }

        private IUserRepository _userRepository;
        private IServiceLogger _logger;
    }
}
