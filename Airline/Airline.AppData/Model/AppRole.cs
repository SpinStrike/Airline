using System.Collections.Generic;

namespace Airline.AppData.Model
{
    /// <summary>
    /// Represent information about application role.
    /// </summary>
    public class AppRole : Entity
    {
        public string Name { get; set; }

        public List<AppUser> Users { get; set; }

        public AppRole()
        {
            Users = new List<AppUser>();
        }
    }
}
