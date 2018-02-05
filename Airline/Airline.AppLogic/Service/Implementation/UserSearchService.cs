using System;
using System.Collections.Generic;
using System.Linq;
using Airline.AppData.Repository;
using Airline.AppLogic.Dto;


namespace Airline.AppLogic.Service.Implementation
{
    public class UserSearchService  : IUserSearchService
    {
        public UserSearchService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public ServiceResult<IEnumerable<UserDto>> FindeBySecondName(string secondName)
        {
            var result = new ServiceResult<IEnumerable<UserDto>>();

            try
            {
                result.Result = _userRepository.GetAll().Where(x => x.SecondName == secondName)
                .ToList()
                .Select(x => x.ToDto(true));
            }
            catch (Exception exc)
            {
                result.Result = new List<UserDto>();
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            return result;
        }

        public ServiceResult<IEnumerable<UserDto>> FindeByEmail(string email)
        {
            var result = new ServiceResult<IEnumerable<UserDto>>();

            try
            {
                result.Result =  _userRepository.GetAll().Where(x => x.Email == email)
                .ToList()
                .Select(x => x.ToDto(true));
            }
            catch (Exception exc)
            {
                result.Result = new List<UserDto>();
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            return result;
        }

        public ServiceResult<IEnumerable<UserDto>> FindeByRole(string role)
        {
            var result = new ServiceResult<IEnumerable<UserDto>>();

            try
            {
                result.Result = _userRepository.GetAll()
                    .ToList()
                    .Where(x => x.Roles.First().Name == role)
                    .Select(x => x.ToDto(true));

                result.Status = AnswerStatus.Success;
            }
            catch (Exception exc)
            {
                result.Result = new List<UserDto>();
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            return result;
        }

        public ServiceResult<IEnumerable<UserDto>> GetAllUsers()
        {
            var result = new ServiceResult<IEnumerable<UserDto>>();

            try
            {
                result.Result = _userRepository.GetAll().ToList()
                    .Select(x => x.ToDto(true));

                result.Status = AnswerStatus.Success;
            }
            catch (Exception exc)
            {
                result.Result = new List<UserDto>();
                result.Status = AnswerStatus.Failure;
                result.Errors.Add("Service error", "Some trouble with getting data. Try Later.");
            }

            return result;
        }

        private IUserRepository _userRepository;
    }
}
