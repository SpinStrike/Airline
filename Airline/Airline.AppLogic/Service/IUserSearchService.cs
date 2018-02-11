using System.Collections.Generic;
using Airline.AppLogic.Dto;

namespace Airline.AppLogic.Service
{
    /// <summary>
    /// Represent functionality to finde user by diffrent parametrs. 
    /// </summary>
    public interface IUserSearchService
    {
        /// <summary>
        /// Finde users by second name.
        /// </summary>
        /// <param name="secondName">User second name</param>
        /// <returns></returns>
        ServiceResult<IEnumerable<UserDto>> FindeBySecondName(string secondName);

        /// <summary>
        /// Finde users by e-mail.
        /// </summary>
        /// <param name="email">User e-mail.</param>
        /// <returns></returns>
        ServiceResult<IEnumerable<UserDto>> FindeByEmail(string email);

        /// <summary>
        /// Finde users by role.
        /// </summary>
        /// <param name="role">User role.</param>
        /// <returns></returns>
        ServiceResult<IEnumerable<UserDto>> FindeByRole(string role);

        /// <summary>
        /// Get all users.
        /// </summary>
        /// <returns></returns>
        ServiceResult<IEnumerable<UserDto>> GetAllUsers();
    }
}
