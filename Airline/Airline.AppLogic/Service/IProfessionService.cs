using Airline.AppLogic.Dto;

namespace Airline.AppLogic.Service
{
    /// <summary>
    /// Represent functionality to work with professios.
    /// </summary>
    public interface IProfessionService : IBaseService<ProfessionDto>
    {
        /// <summary>
        /// Crete new profession.
        /// </summary>
        /// <param name="professionName">Profession name.</param>
        /// <returns></returns>
        ServiceAnswer Create(string professionName);

        /// <summary>
        /// Update profession name.
        /// </summary>
        /// <param name="profession">Ne Profession data.</param>
        /// <returns></returns>
        ServiceAnswer Update(ProfessionDto profession);
    }
}
