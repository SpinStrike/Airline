using System;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;


namespace Airline.AppData.Model
{
    /// <summary>
    /// This class represent information about application user.
    /// </summary>
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public Guid CityId { get; set; }
        public City CurrentLocation { get; set; }

        public List<SendReceiveBroker> RequestBrokers { get; set; }
    }
}
