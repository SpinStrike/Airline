using System;
using System.ComponentModel.DataAnnotations;

namespace Airline.Web.Attributes.Validation
{
    /// <summary>
    /// Validate arrival date of flight.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ArrivalDateValid : ValidationAttribute
    {
        public ArrivalDateValid(string field)
        {
            _field = field;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var arrivalDate = (DateTime)value;
           
                var property = validationContext.ObjectType.GetProperty(_field);

                var deparuteDate = property.GetValue(validationContext.ObjectInstance);
                if (deparuteDate != null)
                {
                    var result = arrivalDate >= (DateTime)deparuteDate;
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