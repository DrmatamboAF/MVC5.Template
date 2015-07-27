using System;
using System.Globalization;

namespace MvcTemplate.Components.Mvc
{
    public class Language
    {
        public String Name { get; set; }
        public String Abbreviation { get; set; }

        public Boolean IsDefault { get; set; }
        public CultureInfo Culture { get; set; }
    }
}
