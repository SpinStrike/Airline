using Airline.AppData.EF;

namespace Airline.AppData.Repository.Implementation
{
    public class DbRepository : IDbRepository
    {
        public DbRepository()
        {
            _dbContext = new AirlineDbContext("AirlineDbConnection");
            _isDisposed = false;
        }

        public AirlineDbContext GetDbInstance()
        {
            lock (_lock)
            {
                if (_dbContext == null)
                {
                    _dbContext = new AirlineDbContext("AirlineDbConnection");
                }

                _isDisposed = false;

                return _dbContext;
            }
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                _dbContext.Dispose();
                _dbContext = null;
            }

            _isDisposed = true;
        }

        private static AirlineDbContext _dbContext;
        private bool _isDisposed;
        private object _lock = new object();
    }
}
