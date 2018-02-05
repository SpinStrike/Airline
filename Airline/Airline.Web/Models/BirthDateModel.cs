using System;
using System.ComponentModel.DataAnnotations;
using Airline.Web.Attributes.Validation;

namespace Airline.Web.Models
{
    public class BirthDateModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Input user age.")]
        [ConfirmMemberAge(ErrorMessage = "Incorrect user age. This value must be between 20-50 years.")]
        public DateTime Date { get; set; }
    }
}