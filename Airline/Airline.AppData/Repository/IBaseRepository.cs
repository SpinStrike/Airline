using System;
using System.Linq;
using Airline.AppData.Model;

namespace Airline.AppData.Repository
{
    public interface IBaseRepository<T> where T : Entity
    {
        void StartTransaction();

        void Commit();

        void RollBack();

        void Delete(T t);

        T FindById(Guid id);

        IQueryable<T> GetAll();
    }
}
