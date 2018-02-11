using System;
using System.ComponentModel.DataAnnotations;
using Airline.Web.Attributes.Validation;

namespace Airline.Web.Areas.Admin.Models
{
    public class UserDataModel
    {
        [Required(ErrorMessage = "First name field can't be empty.")]
        [MinLength(length:2, ErrorMessage = "Minimum length of first name is 2 characters.")]
        [MaxLength(length: 50, ErrorMessage = "Maximum length of first name id 50 characters.")]
        [RegularExpression(pattern: "^[a-zA-Z]+$", ErrorMessage = "First name must contain only a-z and A-Z characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Second name field can't be empty.")]
        [MinLength(length: 2, ErrorMessage = "Minimum length of second name is 2 characters.")]
        [MaxLength(length: 50, ErrorMessage = "Maximum length of second name id 50 characters.")]
        [RegularExpression(pattern: @"^[a-zA-Z]+$", ErrorMessage = "Second name must contain only a-z and A-Z characters.")]
        public string SecondName { get; set; }

        [Required(ErrorMessage = "Input user age.")]
        [ConfirmMemberAge(ErrorMessage = "Incorrect user age. This value must be between 20-50 years.")]
        public DateTime BornDate { get; set; }

        [Required(ErrorMessage = "Phone number field can't be empty.")]
        [RegularExpression(pattern: @"\+[0-9]{12,12}$", ErrorMessage = "Invalid phone number.")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Email field can't be empty.")]
        [RegularExpression(pattern: @"^[a-zA-Z0-9._-]{6,}@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$", ErrorMessage = "Invalid email.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password field can't be empty.")]
        [RegularExpression( pattern: @"^(?=.*[0-9]+)(?=.*[a-z]+)(?=.*[A-Z]+)[a-zA-Z0-9]{6,12}$", ErrorMessage = "Invalid password. It must have length 6-12 characters and contain digit, capital and small letters")]
        public string Password { get; set; }

        [Display(Name = "Confirm password")]
        [Required(ErrorMessage = "Confirm password.")]
        [Compare("Password", ErrorMessage = "Password fields are not matched.")]
        public string ConfirmPasword { get; set; }

        [Required]
        public string Role { get; set; }

        public Guid? CityId { get; set; }

        public Guid? ProfessionId { get; set; }
    }
}