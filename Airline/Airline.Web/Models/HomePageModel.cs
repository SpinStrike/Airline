using Airline.AppLogic.Dto;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Airline.Web.Models
{
    public class HomePageModel
    {
        public IEnumerable<FlightDto> Flights { get; set; } = new List<FlightDto>();

        public IEnumerable<SelectListItem> Cities { get; set; } = new List<SelectListItem>();
    }
}