using Airline.AppLogic.Dto;

namespace Airline.AppLogic.Service
{
    public interface ICityService : IBaseService<CityDto>
    {
        ServiceAnswer Create(string cityName);

        ServiceAnswer Update(CityDto city);
    }
}
