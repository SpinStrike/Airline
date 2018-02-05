using Microsoft.Owin;
using Owin;

using Airline.Web.App_Start;

[assembly: OwinStartup(typeof(Airline.Web.Startup))]

namespace Airline.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            OwinConfiguration.Configuartion(app);
        }    
    }
}
