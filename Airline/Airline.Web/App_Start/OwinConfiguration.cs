using Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Airline.Web.App_Start.Identity;
using Airline.AppData.EF;
using Airline.AppLogic.Service;
using Airline.AppData.Repository;

namespace Airline.Web.App_Start
{
    public class OwinConfiguration
    {
        public static void Configuartion(IAppBuilder app)
        {
            app.CreatePerOwinContext<AirlineDbContext>(AirlineDbContext.Create);
            app.CreatePerOwinContext<AppUserManager>(CreateUserManager);
            app.CreatePerOwinContext<AppSignInManager>(CreateSignInManager);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/SignIn"),
            });
        }

        private static AppUserManager CreateUserManager(IdentityFactoryOptions<AppUserManager> options,
           IOwinContext context)
        {
            var dbContext = context.Get<AirlineDbContext>();
            var store = new AppUserStore(dbContext);

            return new AppUserManager(store);
        }

        private static AppSignInManager CreateSignInManager(IdentityFactoryOptions<AppSignInManager> options,
            IOwinContext context)
        {
            return new AppSignInManager(context.GetUserManager<AppUserManager>(), context.Authentication);
        }

        //public static AppUserManagerWrapper CreateUserManagerWrapper(IdentityFactoryOptions<AppUserManagerWrapper> options,
        //   IOwinContext context)
        //{
        //    var dbContext = context.Get<AirlineDbContext>();
        //    var store = new AppUserStore(dbContext);

        //    return new AppUserManagerWrapper (store);
        //}
    }
}