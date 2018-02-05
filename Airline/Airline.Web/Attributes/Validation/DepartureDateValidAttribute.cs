using System;
using System.ComponentModel.DataAnnotations;

namespace Airline.Web.Attributes.Validation
{
    public class DepartureDateValidAttribute : ValidationAttribute
    {
        public DepartureDateValidAttribute(string field)
        {
            _field = field;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var departureDate = (DateTime)value;
            
                var property = validationContext.ObjectType.GetProperty(_field);

                var arrivalDate = property.GetValue(validationContext.ObjectInstance);
                if (arrivalDate != null)
                {
                    var result = departureDate <= (DateTime)arrivalDate;
                    if (result)
                    {
                        return ValidationResult.Success;
                    }
                }
            }
            return new ValidationResult(ErrorMessage ?? ErrorMessageString);
        }

        private string _field;
    }
}