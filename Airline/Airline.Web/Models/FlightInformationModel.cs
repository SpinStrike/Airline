using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Airline.AppLogic.Dto;

namespace Airline.Web.Models
{
    public class FlightInformationModel
    {
        public FlightDto Flight { get; set; } = new FlightDto(); 

        public IEnumerable<IGrouping<string, AircrewMemberDto>> Members { get; set; } = new List<IGrouping<string, AircrewMemberDto>>();

        public IEnumerable<SelectListItem> AvailableStatuses { get; set; } = new List<SelectListItem>();
    }
}