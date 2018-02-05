using System;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.Owin;
using Airline.AppData.Model;
using Airline.AppLogic.Service;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Airline.Web.App_Start.Identity
{
    public class AppSignInManager : SignInManager<AppUser, Guid>
    {
        public AppSignInManager(AppUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {}

        public SignInStatus EmailPasswordSignIn(string userEmail, string password, bool isPersistent)
        {
            if (UserManager == null)
            {
                return SignInStatus.Failure;
            }
            var user = this.UserManager.FindByEmailAsync(userEmail).Result;
            if (user == null)
            {
                return SignInStatus.Failure;
            }

            if (UserManager.CheckPasswordAsync(user, password).Result)
            {
                SignInAsync(user, isPersistent, false);

                return SignInStatus.Success;
            }

            return SignInStatus.Failure;
        }    
    }
}