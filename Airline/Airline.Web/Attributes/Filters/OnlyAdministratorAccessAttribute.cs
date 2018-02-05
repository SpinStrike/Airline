using System;

namespace Airline.Web.Attributes.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class OnlyAdministratorAccessAttribute : AccessByRoleAttribute
    {
        public OnlyAdministratorAccessAttribute() 
            : base(null)
        {}
    }
}