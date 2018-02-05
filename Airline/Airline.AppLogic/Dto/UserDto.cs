using System;

namespace Airline.AppLogic.Dto
{
    public class UserDto : EntityDto
    {
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public DateTime BornDate { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }
    }
}
