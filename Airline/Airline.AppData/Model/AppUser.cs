using System;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;

namespace Airline.AppData.Model
{
    /// <summary>
    /// Represent information about application user.
    /// </summary>
    public class AppUser : Entity, IUser<Guid>
    {
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public DateTime BornDate { get; set; }

        public string PhoneNumber { get; set; } 

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public string SecurityStamp { get; set; }

        public List<SendReceiveBroker> RequestBrokers { get; set; }

        public List<AppRole> Roles { get; set; }

        public AppUser()
        {
            RequestBrokers = new List<SendReceiveBroker>();
            Roles = new List<AppRole>();
        }
    }
}
