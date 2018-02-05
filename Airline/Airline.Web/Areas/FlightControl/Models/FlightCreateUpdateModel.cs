using System.Collections.Generic;
using Airline.AppLogic.Dto;

namespace Airline.Web.Areas.FlightControl.Models
{
    public class FlightCreateUpdateModel
    {
        public IEnumerable<string> Statuses { get; set; }

        public IEnumerable<CityDto> Cities { get; set; } = new List<CityDto>();

        public FlightDataModel FlightDataModel { get; set; }

        public IEnumerable<AircrewMemberDto> CurrentAircrewMemebers { get; set; } = null;
    }
}