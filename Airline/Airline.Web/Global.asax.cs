using System;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using Ninject.Web.Mvc;
using Airline.Dependency;
using Airline.AppData.Repository;

namespace Airline.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            var dependencies = new AirlineDependencies();
            var kernel = new StandardKernel(dependencies);
            kernel.Unbind<ModelValidatorProvider>();
            DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));
        }

        protected void Application_EndRequest(object sender, EventArgs e)
        {
            var dbContext = DependencyResolver.Current.GetService<IDbRepository>();
            dbContext.Dispose();
        }
    }
}
