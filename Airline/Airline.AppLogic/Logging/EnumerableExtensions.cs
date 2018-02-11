using System.Collections.Generic;
using System.Text;

namespace Airline.AppLogic.Logging
{
    public static class EnumerableExtensions
    {
        public static string GetStringsLits<T>(this IEnumerable<T> list) 
        {
            var resultString = new StringBuilder();

            foreach(var element in list)
            {
                resultString.Append($"{element.ToString()}\r\n");
            }

            return resultString.ToString();
        }  
    }
}
