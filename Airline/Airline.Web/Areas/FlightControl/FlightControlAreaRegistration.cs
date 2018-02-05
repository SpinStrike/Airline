using System.Web.Mvc;

namespace Airline.Web.Areas.FlightControl
{
    public class FlightControlAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "FlightControl";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "FlightControl_default",
                "FlightControl/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                 namespaces: new[] { "Airline.Web.Areas.FlightControl.Controllers" }
            );
        }
    }
}