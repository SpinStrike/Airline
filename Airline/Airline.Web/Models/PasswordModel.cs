using System;
using System.ComponentModel.DataAnnotations;

namespace Airline.Web.Models
{
    public class PasswordModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Password field can't be empty.")]
        [RegularExpression(pattern: @"^(?=.*[0-9]+)(?=.*[a-z]+)(?=.*[A-Z]+)[a-zA-Z0-9]{6,12}$", ErrorMessage = "Invalid password. It must have length 6-12 characters and contain digit, capital and small letters")]
        public string  NewPassword { get; set; }

        [Display(Name = "Confirm password")]
        [Required(ErrorMessage = "Confirm password.")]
        [Compare("NewPassword", ErrorMessage = "Password fields are not matched.")]
        public string  ConfirmPassword { get; set; }
    }
}