using Airline.AppLogic.Dto;

namespace Airline.AppLogic.Service
{
    public interface IProfessionService : IBaseService<ProfessionDto>
    {
        ServiceAnswer Create(string professionName);

        ServiceAnswer Update(ProfessionDto profession);
    }
}
