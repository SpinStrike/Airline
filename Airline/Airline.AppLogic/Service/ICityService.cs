using Airline.AppLogic.Dto;

namespace Airline.AppLogic.Service
{
    /// <summary>
    /// Implement functionality to work with cities. 
    /// </summary>
    public interface ICityService : IBaseService<CityDto>
    {
        /// <summary>
        /// Crete new city.
        /// </summary>
        /// <param name="cityName">City name.</param>
        /// <returns></returns>
        ServiceAnswer Create(string cityName);

        /// <summary>
        /// Update city.
        /// </summary>
        /// <param name="city">New city data.</param>
        /// <returns></returns>
        ServiceAnswer Update(CityDto city);
    }
}
