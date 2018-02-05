using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Airline.AppData.Model;
using Airline.AppData.EF;

namespace Airline.AppData.Repository.Implementation
{
    public class AircrewMemberReository : BaseRepository<AircrewMember> ,IAircrewMemberRepository
    {
        public AircrewMemberReository(IDbRepository dbRepository)
            :base(dbRepository.GetDbInstance(), dbRepository.GetDbInstance().Set<AircrewMember>())
        {
            _dbContext = dbRepository.GetDbInstance();
        }

        public IQueryable<AircrewMember> FindByCurentPosition(Guid idCity)
        {
            return GetAircrewMembersDatatSet().Include(x => x.Profession)
                .Include(x => x.CurrentLocation)
                .Where(x => x.CityId.Equals(idCity));
        }

        public IQueryable<AircrewMember> FindByProfession(Guid idProfession)
        {
            return GetAircrewMembersDatatSet().Include(x => x.Profession)
                .Include(x => x.CurrentLocation)
                .Where(x => x.ProfessionId.Equals(idProfession));
        }

        public IQueryable<AircrewMember> FindBySecondName(string secondName)
        {
            return GetAircrewMembersDatatSet().Include(x => x.Profession)
                .Include(x => x.CurrentLocation)
                .Where(x => x.SecondName.ToUpper().Equals(secondName.ToUpper()));
        }

        public override AircrewMember FindById(Guid aircrewMemberId)
        {
            var targetUser =  GetAircrewMembersDatatSet()
                .Include(x => x.Profession)
                .Include(x => x.CurrentLocation)
                .Include(x => x.Flight)
                .FirstOrDefault(x => x.Id.Equals(aircrewMemberId));

            return targetUser;
        }

        public override IQueryable<AircrewMember> GetAll()
        {
            return GetAircrewMembersDatatSet().Include(x => x.Profession)
                .Include(x => x.CurrentLocation);
        }

        public void SetProfession(Guid aircrewMemberId, Profession profession)
        {
            var targetUser = GetAircrewMembersDatatSet()
                .FirstOrDefault(x => x.Id.Equals(aircrewMemberId));

            targetUser.Profession = profession;
        }

        public void SetCity(Guid aircrewMemberId, City City)
        {
            var targetUser = GetAircrewMembersDatatSet()
                .FirstOrDefault(x => x.Id.Equals(aircrewMemberId));

            targetUser.CurrentLocation = City;
        }

        public void SetStatus(IEnumerable<Guid> aircrewMemberIds, AircrewMemberStatus status)
        {
            var targetUsers = GetAircrewMembersDatatSet().Where(x => aircrewMemberIds.Contains(x.Id));

           foreach (var user in targetUsers)
           {
                user.Status = status;
           }
        }

        private IDbSet<AircrewMember> GetAircrewMembersDatatSet()
        {
            return _dbContext.Set<AircrewMember>();
        }

        private IDbSet<Profession> GetProfessionsDatatSet()
        {
            return _dbContext.Set<Profession>();
        }

        private AirlineDbContext _dbContext;
    }
}
