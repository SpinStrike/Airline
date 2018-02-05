using System.ComponentModel.DataAnnotations;

namespace Airline.Web.Models
{
    public class ChangePasswordModel : PasswordModel
    {
        [Required(ErrorMessage = "Old password field can't be empty.")]
        [RegularExpression(pattern: @"^(?=.*[0-9]+)(?=.*[a-z]+)(?=.*[A-Z]+)[a-zA-Z0-9]{6,12}$", ErrorMessage = "Invalid password. It must have length 6-12 characters and contain digit, capital and small letters")]
        public string OldPassword { get; set; }
    }
}