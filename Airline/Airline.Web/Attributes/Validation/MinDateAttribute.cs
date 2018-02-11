using System;
using System.ComponentModel.DataAnnotations;

namespace Airline.Web.Attributes.Validation
{
    /// <summary>
    /// Validate date. It must be bigger or equal than current date.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class MinDateAttribute : ValidationAttribute
    {
        public MinDateAttribute()
        {
            _minDate = DateTime.Now.Date;
        }

        public override bool IsValid(object value)
        {
            if(value != null)
            {
                try
                {
                    var selectedDate = Convert.ToDateTime(value);

                    if (selectedDate >= _minDate)
                    {
                        return true;
                    }
                }
                catch (Exception)
                {}

                return false;
            }

            return false;
        }

        private DateTime _minDate;
    }
}