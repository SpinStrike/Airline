using Airline.AppData.Model;

namespace Airline.AppLogic.Dto
{
    public class AircrewMemberDto : UserDto
    {
        public CityDto CurrentLocation { get; set; }

        public AircrewMemberStatus Status { get; set; } 

        public ProfessionDto Profession { get; set; }

        public FlightDto Flight { get; set; }
    }
}
