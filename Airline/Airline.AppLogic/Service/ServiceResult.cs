namespace Airline.AppLogic.Service
{
    public class ServiceResult<T> : ServiceAnswer
    {
        public T Result { get; set; }
    }
}
