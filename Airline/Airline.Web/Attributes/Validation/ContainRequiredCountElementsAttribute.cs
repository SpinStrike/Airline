using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Airline.Web.Attributes.Validation
{
    /// <summary>
    /// Validate enumerable objects. They must contain required count of elements.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class ContainRequiredCountElementsAttribute : ValidationAttribute
    {
        public ContainRequiredCountElementsAttribute(int requiredCount)
        {
            _requiredCount = requiredCount;
        }

        public override bool IsValid(object value)
        {
            var collection = (IEnumerable<Guid>)value;

            if(collection.Count() >= _requiredCount)
            {
                return true;
            }

            return false;
        }

        private int _requiredCount;
    }
}