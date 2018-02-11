using System;
using System.Collections.Generic;

namespace Airline.AppLogic.Service
{
    /// <summary>
    /// This interface represent a set of common functions for some services.
    /// </summary>
    /// <typeparam name="T">Service entyty</typeparam>
    public interface IBaseService<T>
    {
        /// <summary>
        /// Delete enity by id.
        /// </summary>
        /// <param name="id">Entity id.</param>
        /// <returns></returns>
        ServiceAnswer Delete(Guid id);

        /// <summary>
        /// Finde entity by id.
        /// </summary>
        /// <param name="id">Entity id.</param>
        /// <returns></returns>
        ServiceResult<T> FindById(Guid id);

        /// <summary>
        /// Get set of all entities.
        /// </summary>
        /// <returns></returns>
        ServiceResult<IEnumerable<T>> GetAll();
    }
}
