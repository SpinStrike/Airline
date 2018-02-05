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
        ServiceAnswer Delete(Guid id);

        ServiceResult<T> FindById(Guid id);

        ServiceResult<IEnumerable<T>> GetAll();
    }
}
