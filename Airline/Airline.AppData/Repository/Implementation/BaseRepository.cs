using System;
using System.Linq;
using System.Data.Entity;
using Airline.AppData.EF;
using Airline.AppData.Model;

namespace Airline.AppData.Repository.Implementation
{
    public class BaseRepository<T>  where T : Entity
    {
        public BaseRepository(AirlineDbContext dbContext, IDbSet<T> dbSet)
        {
            _dbContext = dbContext;
            _dbSet = dbSet;
            _currentTransaction = null;
        }

        public void StartTransaction()
        {
            _currentTransaction = _dbContext.Database.BeginTransaction();
        }

        public void Commit()
        {
            _dbContext.SaveChanges();
            _currentTransaction.Commit();

            if (_currentTransaction != null)
            {
                _currentTransaction.Dispose();
            }
        }

        public void RollBack()
        {
            if (_currentTransaction != null)
            {
                _currentTransaction.Rollback();
                _currentTransaction.Dispose();
            }
        }

        public void Delete(T t)
        {
            _dbSet.Remove(t);
        }

        public virtual T FindById(Guid id)
        {
            return _dbSet.FirstOrDefault(x => x.Id.Equals(id));
        }

        public virtual IQueryable<T> GetAll()
        {
            return _dbSet;
        }

        protected void Add(T t)
        {
            _dbSet.Add(t);
        }


        protected AirlineDbContext GetContext()
        {
            return _dbContext;
        }


        private IDbSet<T> _dbSet;
        private AirlineDbContext _dbContext;
        private DbContextTransaction _currentTransaction;
    }
}
