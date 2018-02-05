using System.Collections.Generic;
using Airline.AppLogic.Dto;


namespace Airline.Web.Models
{
    public class AircrewMemberModel
    {
        public AircrewMemberDto User { get; set; }

        public IEnumerable<CityDto> Cities { get; set; } = new List<CityDto>();

        public IEnumerable<ProfessionDto> Professions { get; set; } = new List<ProfessionDto>();

        public string FlightNumber { get; set; } = null;
    }
}