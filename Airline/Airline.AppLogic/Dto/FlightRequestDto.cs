using System;
using Airline.AppData.Model;

namespace Airline.AppLogic.Dto
{
    public class FlightRequestDto : EntityDto
    {
        public bool IsReaded { get; set; }

        public string Message { get; set; }

        public AdminAnswerStatus Status { get; set; }

        public DateTime SendTime { get; set; }

        public UserDto From { get; set; }
       
        public UserDto To { get; set; }
    }
}
