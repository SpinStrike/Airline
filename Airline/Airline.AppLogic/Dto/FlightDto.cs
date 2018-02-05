using System;
using System.Collections.Generic;
using Airline.AppData.Model;

namespace Airline.AppLogic.Dto
{
    public class FlightDto : EntityDto
    {
        public string Number { get; set; }

        public string Name { get; set; }

        public DateTime DepartureDate { get; set; }

        public DateTime ArrivalDate { get; set; }

        public CityDto From { get; set; }

        public CityDto To { get; set; }

        public FlightStatus Status { get; set; }

        public IEnumerable<AircrewMemberDto> AircrewMembers { get; set; }
    }

    public static class FlightStatusConvertation   
    {
        public static string GetDescription(this FlightStatus status)
        {
            return status == FlightStatus.InAir ? "In Air" : status.ToString();
        }      
    }
}
