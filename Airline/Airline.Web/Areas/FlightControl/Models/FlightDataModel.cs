using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Airline.Web.Attributes.Validation;

namespace Airline.Web.Areas.FlightControl.Models
{
    public class FlightDataModel
    {
        public Guid? Id { get; set; } = null;

        [Required(ErrorMessage = "Number field can't be empty")]
        [RegularExpression(pattern:@"^[A-Z]{2,3}[0-9]{4,4}$", ErrorMessage = "Invalid flight number")]
        public string Number { get; set; }

        [Required(ErrorMessage = "From city field can't be empty")]
        [NotEqualCities(field: "ToCity", ErrorMessage = "Points from city and to city must have different value.")]
        public Guid FromCity { get; set; }

        [Required(ErrorMessage = "To city field can't be empty")]
        [NotEqualCities(field: "FromCity", ErrorMessage = "Points to city and from city must have different value.")]
        public Guid ToCity { get; set; }

        [Required(ErrorMessage = "Departure date field can't be empty")]
        [DepartureDateValid("ArrivalDate", ErrorMessage = "Departure date must be earlier than arrival.")]
        public DateTime DepartureDate { get; set; }

        [Required(ErrorMessage = "Arrival date field can't be empty")]
        [ArrivalDateValid("DepartureDate", ErrorMessage = "Arrival date must be later than departure.")]
        public DateTime ArrivalDate { get; set; }

        public string Status { get; set; }

        [ContainRequiredCountElements(2, ErrorMessage = "Flight required minimum 2 pilots.")]
        public List<Guid> Pilots { get; set; } = new List<Guid>();

        [ContainRequiredCountElements(1, ErrorMessage = "Flight required minimum 1 aircraft navigator.")]
        public List<Guid> AircraftNavigators { get; set; } = new List<Guid>();

        [ContainRequiredCountElements(1, ErrorMessage = "Flight required minimum 1 radio operator.")]
        public List<Guid> RadioOperators { get; set; } = new List<Guid>();

        [ContainRequiredCountElements(1, ErrorMessage = "Flight required minimum 1 flight engineer.")]
        public List<Guid> FlightEngineers { get; set; } = new List<Guid>();

        [ContainRequiredCountElements(3, ErrorMessage = "Flight required minimum 3 stewardesses.")]
        public List<Guid> Stewardesses { get; set; } = new List<Guid>();
    }
}