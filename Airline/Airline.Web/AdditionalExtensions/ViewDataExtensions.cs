using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Airline.Web.AdditionalExtensions
{
    public static class ViewDataExtensions
    {
        public static string GetErrorMessageByKey(this ViewDataDictionary dataDictionary, string key)
        {
            var keyValues = dataDictionary.ModelState.FirstOrDefault(x => x.Key == key);

            if(keyValues.Value != null)
            {
                return keyValues.Value.Errors.First().ErrorMessage;
            }

            return string.Empty;
        }
    }
}