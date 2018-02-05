using Airline.AppData.Model;

namespace Airline.AppData.Repository.Implementation
{
    public class ProfessionRepository : BaseRepository<Profession>, IProfessionRepository
    {
        public ProfessionRepository(IDbRepository dbRepository)
            :base(dbRepository.GetDbInstance(), dbRepository.GetDbInstance().Professions)
        {}

        public void Create(string professionName)
        {
            var profession = new Profession()
            {
                Name = professionName
            };

            this.Add(profession);
        }
    }
}
