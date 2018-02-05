using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Airline.AppData.Model;
using Airline.AppData.Repository;


namespace Airline.AppLogic.Service
{
    public class AppUserManager : UserManager<AppUser, Guid>
    {
        public AppUserManager(IUserStore<AppUser, Guid> userStore)
            : base(userStore)
        {
            ClaimsIdentityFactory = new AppClaimIdentityFactory();

            UserValidator = new UserValidator<AppUser, Guid>(this)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true,
            };

            PasswordValidator = new PasswordValidator()
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            UserTokenProvider = new EmailTokenProvider<AppUser, Guid>();
        }

        public IdentityResult SetFirstName(Guid userId, string name)
        {
            var store = GetUserPersonalDataStore();
            var user = FindByIdAsync(userId).Result;
            if (user == null)
            {
                throw new InvalidOperationException("User is not found.");
            }
            store.SetFirstName(user, name);

            return UpdateAsync(user).Result;
        }

        public IdentityResult SetSecondName(Guid userId, string surname)
        {
            var store = GetUserPersonalDataStore();
            var user = FindByIdAsync(userId).Result;
            if (user == null)
            {
                throw new InvalidOperationException("User is not found.");
            }
            store.SetSecondName(user, surname);

            return UpdateAsync(user).Result;
        }

        public IdentityResult SetBornDate(Guid userId, DateTime dateTime)
        {
            var store = GetUserPersonalDataStore();
            var user = FindByIdAsync(userId).Result;
            if (user == null)
            {
                throw new InvalidOperationException("User is not found.");
            }
            store.SetBornDate(user, dateTime);

            return UpdateAsync(user).Result;
        }

        public override Task<IdentityResult> SetEmailAsync(Guid userId, string email)
        {
            var user = FindByIdAsync(userId).Result;
            if (user == null)
            {
                throw new InvalidOperationException("User is not found.");
            }

            user.UserName = email;

            var emailStore = GetEmailStore();
            emailStore.SetEmailAsync(user, email).Wait();

            var result = UserValidator.ValidateAsync(user).Result;
            if(result.Succeeded)
            {
                return UpdateAsync(user);
            }

            return Task.FromResult(result);
        }

        internal IUserPersonalDataStore GetUserPersonalDataStore()
        {
            return Store as IUserPersonalDataStore;
        }

        internal IUserEmailStore<AppUser, Guid> GetEmailStore()
        {
            return Store as IUserEmailStore<AppUser, Guid>;
        }
    }
}
