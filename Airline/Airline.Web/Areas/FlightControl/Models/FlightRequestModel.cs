using System;
using System.ComponentModel.DataAnnotations;

namespace Airline.Web.Areas.FlightControl.Models
{
    public class FlightRequestModel
    {
        [Required(ErrorMessage = "Required select sender.")]
        public string From { get; set; }

        [Required(ErrorMessage = "Required select recipient.")]
        [RegularExpression(pattern: @"^[a-zA-Z0-9._-]{6,}@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$", ErrorMessage = "Invalid recipient email.")]
        public string To { get; set; }

        [Required(ErrorMessage = "Flight request message can't be empty.")]
        public string Message { get; set; }

        public string Status { get; set; }

        public DateTime Date { get; set; }

        public Guid Id;
    }
}