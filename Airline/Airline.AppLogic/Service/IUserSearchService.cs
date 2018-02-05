using System.Collections.Generic;
using Airline.AppLogic.Dto;

namespace Airline.AppLogic.Service
{
    public  interface IUserSearchService
    {
        ServiceResult<IEnumerable<UserDto>> FindeBySecondName(string secondName);

        ServiceResult<IEnumerable<UserDto>> FindeByEmail(string email);

        ServiceResult<IEnumerable<UserDto>> FindeByRole(string email);

        ServiceResult<IEnumerable<UserDto>> GetAllUsers();
    }
}
