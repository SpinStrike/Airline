namespace Airline.AppLogic.Service
{
    /// <summary>
    /// Represent extension for service answer. Contain property to return object from service functions.
    /// </summary>
    /// <typeparam name="T">Return object.</typeparam>
    public class ServiceResult<T> : ServiceAnswer
    {
        public T Result { get; set; }
    }
}
