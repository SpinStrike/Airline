using System;
using System.ComponentModel.DataAnnotations;

namespace Airline.Web.Attributes.Validation
{
    public class ConfirmMemberAgeAttribute : ValidationAttribute
    {
        public ConfirmMemberAgeAttribute()
        {
            _minDate = DateTime.Now.AddYears(-50);
            _maxDate = DateTime.Now.AddYears(-20);
        }

        public override bool IsValid(object value)
        {
            var targetDate = (DateTime)value;

            var isBiggerMinDate = targetDate.Date >= _minDate.Date;
            var isLessMaxDate = targetDate.Date <= _maxDate.Date;

            if (isBiggerMinDate && isLessMaxDate)
            {
                return true;
            }

            return false;
        }

        public override string FormatErrorMessage(string name)
        {
            return base.FormatErrorMessage(name);
        }

        private DateTime _minDate;
        private DateTime _maxDate;
    }
}