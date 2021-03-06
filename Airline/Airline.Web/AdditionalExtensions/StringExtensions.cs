﻿using System;
using System.Collections.Generic;

namespace Airline.Web.AdditionalExtensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// Split text or paragraphs.
        /// </summary>
        public static IEnumerable<string> SplitToParagraphs(this string s)
        {
            var resultSet = new List<string>(s.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries));

            return resultSet;
        }
    }
}