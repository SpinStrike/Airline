using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace Airline.Web.Attributes.Filters
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class AccessByRoleAttribute : FilterAttribute, IActionFilter
    {
        public AccessByRoleAttribute(string role)
        {
            _role = role;
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            var user = filterContext.HttpContext.User;
            var isInRequiredRole = false;

            if(user.Identity.IsAuthenticated)
            {
               if(_role != null && user.IsInRole(_role))
               {
                    isInRequiredRole = true;
               }
               else if(user.IsInRole("Administrator"))
               {
                    isInRequiredRole = true;
               }
            }

            if (!isInRequiredRole)
            {
                filterContext.Result = 
                    new RedirectToRouteResult(new RouteValueDictionary {
                        { "action", "NotAllowAccess" },
                        { "controller", "Home" },
                        { "area", "" }
                        });
            }
        }

        public void OnActionExecuting(ActionExecutingContext filterContext) {}

        private string _role;
    }
}