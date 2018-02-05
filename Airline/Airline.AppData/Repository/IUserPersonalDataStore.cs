using System;
using Microsoft.AspNet.Identity;
using Airline.AppData.Model;

namespace Airline.AppData.Repository
{
    public interface IUserPersonalDataStore : IUserStore<AppUser, Guid>
    {
        void SetFirstName(AppUser user, string firstName);

        void SetSecondName(AppUser user, string secondName);

        void SetBornDate(AppUser user, DateTime bornDate);
    }
}
