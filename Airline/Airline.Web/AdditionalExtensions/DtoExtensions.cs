using System.Linq;
using Airline.Web.Models;
using Airline.AppLogic.Dto;
using System.Collections.Generic;

namespace Airline.Web.AdditionalExtensions
{
    public static  class DtoExtensions
    {
        /// <summary>
        /// Group by profession and sort into groups by full name flight aircre members.
        /// </summary>
        /// <param name="flight">Flight</param>
        /// <returns></returns>
        public static FlightInformationModel GetStructetFlightDto(this FlightDto flight)
        {
            var model = new FlightInformationModel();

            if (!(flight.AircrewMembers.Count() == 0) || !(flight.AircrewMembers == null))
            {
                var members = flight.AircrewMembers.OrderBy(x => x.SecondName)
                    .GroupBy(x => x.Profession.Name);
                flight.AircrewMembers = null;

                model.Flight = flight;
                model.Members = members.OrderBy(x => x.Key, new ProfessionComparer());
            }

            return model;
        }

        /// <summary>
        /// Sort professions by flight seniority.
        /// </summary>
        private class ProfessionComparer : IComparer<string>
        {
            public int Compare(string x, string y)
            {
                return GetPriority(x).CompareTo(GetPriority(y));
            }

            private int GetPriority(string s)
            {
                switch (s)
                {
                    case "Pilot": return 0;
                    case "Aircraft Navigator":
                        return 1;
                    case "Radio Operator":
                        return 2;
                    case "Flight Engineer":
                        return 3;
                    case "Stewardess":
                        return 4;
                    default:
                        return 5;
                }
            }
        }
    }
}