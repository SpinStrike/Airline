using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Airline.AppData.Model;
using Airline.AppData.Repository;
using Airline.AppLogic.Logging;

namespace Airline.AppLogic.Service
{
    public class AppUserManager : UserManager<AppUser, Guid>
    {
        public AppUserManager(IUserStore<AppUser, Guid> userStore, IServiceLogger logger)
            : base(userStore)
        {
            _logger = logger;

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

        public override async Task<IdentityResult> CreateAsync(AppUser user, string password)
        {
            _logger.Debug("Start crete user method.");

            IdentityResult result = null;

            try
            {
                result = await base.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    var userAsString = new StringBuilder();

                    userAsString.Append($"Identifier: {user.Id}\r\n");
                    userAsString.Append($"Full name: {user.FirstName} {user.SecondName}\r\n");
                    userAsString.Append($"Born date: {user.BornDate.ToString("dd-MM-yyyy")}\r\n");
                    userAsString.Append($"E-mail: {user.Email}\r\n");
                    userAsString.Append($"Phone number: {user.PhoneNumber}");

                    _logger.Info($"Added new user:\r\n {userAsString.ToString()}");
                }
                else
                {
                    _logger.Warning($"User was not added.\r\n Occurred errors:\r\n {result.Errors.GetStringsLits()}");
                }
            }
            catch (Exception exc)
            {
                _logger.Error($"Exception occurred during the addition of a new user.\r\n Exception: {exc.ToString()}");

                result = new IdentityResult(new string[] { "Some service problem. Try later." });
            }

            _logger.Debug("Finish crete user method.");

            return await Task.FromResult<IdentityResult>(result);
        }

        public override async Task<IdentityResult> AddToRoleAsync(Guid userId, string role)
        {
            _logger.Debug("Start add user to role method.");

            IdentityResult result = null;

            try
            {
                result = await base.AddToRoleAsync(userId, role);
                if(result.Succeeded)
                {
                    _logger.Info($"User (Id: {userId}) was set new role: '{role}'");
                }
                else
                {
                    _logger.Warning($"User (Id: {userId}) was not set new role: '{role}'.\r\n Occurred errors:\r\n {result.Errors.GetStringsLits()}");
                }
            }
            catch(Exception exc)
            {
                _logger.Error($"Exception occurred during the addition of a user, id: {userId}, to role {role}.\r\n Exception: {exc.ToString()}");

                result = new IdentityResult(new string[] { "Some service problem. Try later." });
            }

            _logger.Debug("Finish add user to role method.");

            return await Task.FromResult<IdentityResult>(result);
        }

        public IdentityResult SetFirstName(Guid userId, string name)
        {
            _logger.Debug("Start set user first name method.");

            IdentityResult result = null;

            try
            {
                var store = GetUserPersonalDataStore();
                var user = FindByIdAsync(userId).Result;
                if (user == null)
                {
                    _logger.Warning($"User (Id: {userId}) was not found.");

                    result = new IdentityResult(new string[] { "User was not found." });

                    _logger.Debug("Finish set user first name method.");

                    return result;
                }

                store.SetFirstName(user, name);

                result = UpdateAsync(user).Result;
                if(result.Succeeded)
                {
                    _logger.Info($"To user (Id: {userId}) was set new first name: '{name}'");
                }
                else
                {
                    _logger.Warning($"First name of user (Id: {userId}) was not changed.\r\n Occurred errors:\r\n {result.Errors.GetStringsLits()}");
                }
            }
            catch(Exception exc)
            {
                _logger.Error($"Exception occurred during the setting first name to user, id: {userId}.\r\n Exception: {exc.ToString()}");

                result = new IdentityResult(new string[] { "Some service problem. Try later." });
            }

            _logger.Debug("Finish set user first name method.");

            return result;
        }

        public IdentityResult SetSecondName(Guid userId, string surname)
        {
            _logger.Debug("Start set user second name method.");

            IdentityResult result = null;

            try
            {
                var store = GetUserPersonalDataStore();
                var user = FindByIdAsync(userId).Result;
                if (user == null)
                {
                    _logger.Warning($"User (Id: {userId}) was not found.");

                    result = new IdentityResult(new string[] { "User was not found." });

                    _logger.Debug("Finish set user second name method.");

                    return result;
                }
                store.SetSecondName(user, surname);

                result =  UpdateAsync(user).Result;
                if(result.Succeeded)
                {
                    _logger.Info($"To user (Id: {userId}) was set new second name: '{surname}'");
                }
                else
                {
                    _logger.Warning($"Second name of user (Id: {userId}) was not changed.\r\nOccurred errors:\r\n {result.Errors.GetStringsLits()}");
                }
            }
            catch(Exception exc)
            {
                _logger.Error($"Exception occurred during the setting second name to user, id: {userId}.\r\nException: {exc.ToString()}");

                result = new IdentityResult(new string[] { "Some service problem. Try later." });
            }

            _logger.Debug("Finish set user second name method.");

            return result;
        }

        public IdentityResult SetBornDate(Guid userId, DateTime dateTime)
        {
            _logger.Debug("Start set user born date method.");

            IdentityResult result = null;

            try
            {
                var store = GetUserPersonalDataStore();
                var user = FindByIdAsync(userId).Result;
                if (user == null)
                {
                    _logger.Warning($"User (Id: {userId}) was not found.");

                    result = new IdentityResult(new string[] { "User was not found." });

                    _logger.Debug("Finish set user born date method.");

                    return result;
                }
                store.SetBornDate(user, dateTime);

                result = UpdateAsync(user).Result;
                if(result.Succeeded)
                {
                    _logger.Info($"To user (Id: {userId}) was set born date: '{dateTime.ToString("dd-MM-yyyy")}'");
                }
                else
                {
                    _logger.Warning($"Born date of user (Id: {userId}) was not changed.\r\n Occurred errors:\r\n {result.Errors.GetStringsLits()}");
                }
            }
            catch(Exception exc)
            {
                _logger.Error($"Exception occurred during the setting born date to user, id: {userId}.\r\n Exception: {exc.ToString()}");

                result = new IdentityResult(new string[] { "Some service problem. Try later." });
            }

            _logger.Debug("Finish set user born date method.");

            return result;
        }

        public override async Task<IdentityResult> SetEmailAsync(Guid userId, string email)
        {
            _logger.Debug("Start set user e-mail method.");

            IdentityResult result = null;

            try
            {
                var user = await FindByIdAsync(userId);
                if (user == null)
                {
                    _logger.Warning($"User (Id: {userId}) was not found.");

                    result = new IdentityResult(new string[] { "User was not found." });

                    _logger.Debug("Finish set user born date method.");

                    return result;
                }

                var emailStore = GetEmailStore();
                await emailStore.SetEmailAsync(user, email);
                user.UserName = email;

                 result = await UserValidator.ValidateAsync(user);
                if (result.Succeeded)
                {
                    result = await UpdateAsync(user);
                    if(result.Succeeded)
                    {
                        _logger.Info($"To user (Id: {userId}) was set new e-mail: '{email}'");
                    }
                    else
                    {
                        _logger.Warning($"Email of user (Id: {userId}) was not changed.\r\n Occurred errors:\r\n {result.Errors.GetStringsLits()}");
                    }
                }
                else
                {
                    _logger.Warning($"Email failed validation: '{email}'.\r\n Occurred errors:\r\n {result.Errors.GetStringsLits()}");
                }
            }
            catch(Exception exc)
            {
                _logger.Error($"Exception occurred during the setting email to user, id: {userId}.\r\n Exception: {exc.ToString()}");

                result = new IdentityResult(new string[] { "Some service problem. Try later." });
            }

            _logger.Debug("Finish set user email method.");

            return await Task.FromResult(result);
        }

        public override async Task<IdentityResult> SetPhoneNumberAsync(Guid userId, string phoneNumber)
        {
            _logger.Debug("Start set user phone number method.");

            IdentityResult result = null;

            try
            {
                result = await base.SetPhoneNumberAsync(userId, phoneNumber);
                if(result.Succeeded)
                {
                    _logger.Info($"To user (Id: {userId}) was set new phone number: '{phoneNumber}'");
                }
                else
                {
                    _logger.Warning($"Phone number of user (Id: {userId}) was not changed.\r\n Occurred errors:\r\n {result.Errors.GetStringsLits()}");
                }
            }
            catch(Exception exc)
            {
                _logger.Error($"Exception occurred during the setting phone number to user, id: {userId}.\r\n Exception: {exc.ToString()}");

                result = new IdentityResult(new string[] { "Some service problem. Try later."});
            }

            _logger.Debug("Finish set user phone number method.");

            return result;
        }

        public override async Task<IdentityResult> ChangePasswordAsync(Guid userId, string currentPassword, string newPassword)
        {
            _logger.Debug("Start change user password method.");

            IdentityResult result = null;

            try
            {
                result = await base.ChangePasswordAsync(userId, currentPassword, newPassword);
                if(result.Succeeded)
                {
                    _logger.Info($"To user (Id: {userId}) was set new password: '{newPassword}'");
                }
                else
                {
                    _logger.Warning($"Password of user (Id: {userId}) was not changed.\r\n Occurred errors:\r\n {result.Errors.GetStringsLits()}");
                }
            }
            catch(Exception exc)
            {
                _logger.Error($"Exception occurred during the setting change user password, id: {userId}.\r\n Exception: {exc.ToString()}");

                result = new IdentityResult(new string[] { "Some service problem. Try later." });
            }

            _logger.Debug("Finish change user password method.");

            return result;
        }

        public override async Task<IdentityResult> ResetPasswordAsync(Guid userId, string token, string newPassword)
        {
            _logger.Debug("Start reset user password method.");

            IdentityResult result = null;

            try
            {
                result = await base.ResetPasswordAsync(userId, token, newPassword);
                if (result.Succeeded)
                {
                    _logger.Info($"To user (Id: {userId}) was reset password. New password: '{newPassword}'");
                }
                else
                {
                    _logger.Warning($"Password of user (Id: {userId}) was not reseted.\r\n Occurred errors:\r\n {result.Errors.GetStringsLits()}");
                }
            }
            catch(Exception exc)
            {
                _logger.Error($"Exception occurred during the resetting user password, id: {userId}.\r\n Exception: {exc.ToString()}");

                result = new IdentityResult(new string[] { "Some service problem. Try later." });
            }

            _logger.Debug("Finish reset user password method.");

            return result;
        }

        public override async Task<IdentityResult> DeleteAsync(AppUser user)
        {
            _logger.Debug("Start delete user method.");

            IdentityResult result = null;

            try
            {
                result = await base.DeleteAsync(user);
                if(result.Succeeded)
                {
                    _logger.Info($"To user (Id: {user.Id}) has been deleted.");
                }
                else
                {
                    _logger.Warning($"User (Id: {user.Id}) was not reseted.\r\n Occurred errors:\r\n {result.Errors.GetStringsLits()}");
                }
            }
            catch(Exception exc)
            {
                _logger.Error($"Exception occurred during the deletting user password, id: {user.Id}.\r\n Exception: {exc.ToString()}");

                result = new IdentityResult(new string[] { "Some service problem. Try later." });
            }

            _logger.Debug("Finish delete user method.");

            return result;
        }

        internal IUserPersonalDataStore GetUserPersonalDataStore()
        {
            return Store as IUserPersonalDataStore;
        }

        internal IUserEmailStore<AppUser, Guid> GetEmailStore()
        {
            return Store as IUserEmailStore<AppUser, Guid>;
        }

        private IServiceLogger _logger;
    }
}
