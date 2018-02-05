using System;
using System.ComponentModel.DataAnnotations;

namespace Airline.Web.Models
{
    public class PhoneNumberModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Phone number field can't be empty.")]
        [RegularExpression(pattern: @"[+]?[0-9]{12,12}$", ErrorMessage = "Invalid phone number.")]
        public string PhoneNumber { get; set; }
    }
}