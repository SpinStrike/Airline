using System;

namespace Airline.Web.Attributes.Filters
{
    /// <summary>
    /// Allow access to website resources only user in administrator role.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class OnlyAdministratorAccessAttribute : AccessByRoleAttribute
    {
        public OnlyAdministratorAccessAttribute() 
            : base(null)
        {}
    }
}