using System;

namespace Airline.Web.Models
{
    public class UserModel
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public DateTime DirthDate { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }
    }
}