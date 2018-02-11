using System;
using System.Linq;
using System.Collections.Generic;
using Airline.AppData.Model;

namespace Airline.AppData.Repository
{
    public interface IAircrewMemberRepository : IBaseRepository<AircrewMember>
    {
        IQueryable<AircrewMember> FindBySecondName(string secondName);

        IQueryable<AircrewMember> FindByProfession(Guid idProfession);

        IQueryable<AircrewMember> FindByCurentPosition(Guid idCity);

        void SetStatus(IEnumerable<Guid> aircrewMemberIds, AircrewMemberStatus status);

        void SetProfession(AircrewMember aircrewMember, Profession profession);

        void SetCity(AircrewMember aircrewMember, City City);
    }
}
