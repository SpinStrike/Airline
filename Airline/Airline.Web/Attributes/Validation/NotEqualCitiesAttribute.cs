using System;
using System.ComponentModel.DataAnnotations;

namespace Airline.Web.Attributes.Validation
{
    /// <summary>
    /// Validate inequality of two cities.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class NotEqualCitiesAttribute : ValidationAttribute
    {
        public NotEqualCitiesAttribute(string field)
        {
            _field = field;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var currentCity = (Guid)value;

            var property = validationContext.ObjectType.GetProperty(_field);

            var selectedCity = property.GetValue(validationContext.ObjectInstance);
            if (selectedCity == null)
            {
                throw new ArgumentException("Property is not found");
            }

            var result = currentCity != (Guid)selectedCity;
            if (result)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage ?? ErrorMessageString);
        }

        private string _field;
    }
}