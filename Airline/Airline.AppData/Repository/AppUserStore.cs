using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Airline.AppData.Model;
using Airline.AppData.EF;

namespace Airline.AppData.Repository
{
    public class AppUserStore : IUserStore<AppUser, Guid>,
        IUserPasswordStore<AppUser, Guid>,
        IUserEmailStore<AppUser, Guid>,
        IUserPhoneNumberStore<AppUser, Guid>,
        IUserSecurityStampStore<AppUser, Guid>,
        IUserRoleStore<AppUser, Guid>,
        IUserLockoutStore<AppUser, Guid>,
        IUserTwoFactorStore<AppUser, Guid>,
        IUserPersonalDataStore
    {
        public AppUserStore(AirlineDbContext dbContext)
        {
            _dbContext = dbContext;
            _isDisposed = false;
        }

        public static IUserStore<AppUser, Guid> Crete(AirlineDbContext dbContext)
        {
            return new AppUserStore(dbContext);
        }

        public Task CreateAsync(AppUser user)
        {
            GetUserDataSet().Add(user);
           _dbContext.SaveChanges();
       
            return Task.FromResult(0);
        }

        public Task DeleteAsync(AppUser user)
        {
            GetUserDataSet().Remove(user);
            _dbContext.SaveChanges();

            return Task.FromResult(0);
        }

        public Task<AppUser> FindByIdAsync(Guid userId)
        {
            var targetUser = GetUserDataSet().FirstOrDefault(x => x.Id.Equals(userId));

            return Task.FromResult(targetUser);
        }

        public Task<AppUser> FindByNameAsync(string userName)
        {
            var targetUser = GetUserDataSet().FirstOrDefault(x => x.UserName == userName);

            return Task.FromResult(targetUser);
        }

        public Task UpdateAsync(AppUser user)
        {
            var targetUser = GetUserDataSet().FirstOrDefault(x => x.Id.Equals(user.Id));

            targetUser = user;
            _dbContext.SaveChanges();

            return Task.FromResult(0);
        }

        public void Dispose()
        {
            if(!_isDisposed)
            {
                _dbContext.Dispose();
                _dbContext = null;
            }

            _isDisposed = true;
        }

        public Task SetPasswordHashAsync(AppUser user, string passwordHash)
        {
            user.PasswordHash = passwordHash;

            return Task.FromResult(0);
        }

        public Task<string> GetPasswordHashAsync(AppUser user)
        {
            var passworHash = user.PasswordHash;

            return Task.FromResult(passworHash);
        }

        public Task<bool> HasPasswordAsync(AppUser user)
        {
            var isHavePassworHash = user.PasswordHash != null;

            return Task.FromResult(isHavePassworHash);
        }

        public Task SetEmailAsync(AppUser user, string email)
        {
            user.Email = email;

            return Task.FromResult(0);
        }

        public Task<string> GetEmailAsync(AppUser user)
        {
            var email = user.Email;

            return Task.FromResult(email);
        }

        public Task<bool> GetEmailConfirmedAsync(AppUser user)
        {
            return Task.FromResult(true);
        }

        public Task SetEmailConfirmedAsync(AppUser user, bool confirmed)
        {
            return Task.FromResult(true); ;
        }

        public Task<AppUser> FindByEmailAsync(string email)
        {
            var targetUser = GetUserDataSet().FirstOrDefault(x => x.Email == email);

            return Task.FromResult(targetUser);
        }

        public Task SetPhoneNumberAsync(AppUser user, string phoneNumber)
        {
            user.PhoneNumber = phoneNumber;

            return Task.FromResult(0);
        }

        public Task<string> GetPhoneNumberAsync(AppUser user)
        {
            var phoneNumber = user.PhoneNumber;

            return Task.FromResult(phoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(AppUser user)
        {
            return Task.FromResult(true);
        }

        public Task SetPhoneNumberConfirmedAsync(AppUser user, bool confirmed)
        {
            return Task.FromResult(true);
        }

        public Task SetSecurityStampAsync(AppUser user, string stamp)
        {
            user.SecurityStamp = stamp;

            return Task.FromResult(0);
        }

        public Task<string> GetSecurityStampAsync(AppUser user)
        {
            var securityStamp = user.SecurityStamp;

            return Task.FromResult(securityStamp);
        }

        public Task AddToRoleAsync(AppUser user, string roleName)
        {
            var targetRole = GetRoleDatSet().FirstOrDefault(x => x.Name.Equals(roleName));

            user.Roles.Add(targetRole);

            return Task.FromResult(0);
        }

        public Task RemoveFromRoleAsync(AppUser user, string roleName)
        {
            var targetUser = GetUserDataSet().Include(x => x.Roles).
                FirstOrDefault(x => x.Id.Equals(user.Id));

            user.Roles.Remove(user.Roles.FirstOrDefault(x => x.Name.Equals(roleName)));

            return Task.FromResult(0);
        }

        public Task<IList<string>> GetRolesAsync(AppUser user)
        {
            var roles = GetUserDataSet().Include(x => x.Roles)
               .FirstOrDefault(x => x.Id.Equals(user.Id))
               .Roles.Select(x => x.Name)
               .ToList();

            return Task.FromResult(roles as IList<string>);
        }

        public Task<bool> IsInRoleAsync(AppUser user, string roleName)
        {
            var isInRole = GetUserDataSet().Include(x => x.Roles)
               .FirstOrDefault(x => x.Id.Equals(user.Id))
               .Roles.Select(x => x.Name)
               .Contains(roleName);

            return Task.FromResult(isInRole);
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(AppUser user)
        {
            throw new NotImplementedException();
        }

        public Task SetLockoutEndDateAsync(AppUser user, DateTimeOffset lockoutEnd)
        {
            throw new NotImplementedException();
        }

        public Task<int> IncrementAccessFailedCountAsync(AppUser user)
        {
            throw new NotImplementedException();
        }

        public Task ResetAccessFailedCountAsync(AppUser user)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetAccessFailedCountAsync(AppUser user)
        {
            return Task.FromResult(0);
        }

        public Task<bool> GetLockoutEnabledAsync(AppUser user)
        {
            return Task.FromResult(false);
        }

        public Task SetLockoutEnabledAsync(AppUser user, bool enabled)
        {
            throw new NotImplementedException();
        }

        public Task SetTwoFactorEnabledAsync(AppUser user, bool enabled)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetTwoFactorEnabledAsync(AppUser user)
        {
            return Task.FromResult(false);
        }

        public void SetFirstName(AppUser user, string firstName)
        {
            user.FirstName = firstName;
        }

        public void SetSecondName(AppUser user, string secondName)
        {
            user.SecondName = secondName;
        }

        public void SetBornDate(AppUser user, DateTime bornDate)
        {
            user.BornDate = bornDate;
        }

        private IDbSet<AppUser> GetUserDataSet()
        {
            return _dbContext.Users;
        }

        private IDbSet<AppRole> GetRoleDatSet()
        {
            return _dbContext.Roles;
        }

        private AirlineDbContext _dbContext;
        private bool _isDisposed;
    }
}
