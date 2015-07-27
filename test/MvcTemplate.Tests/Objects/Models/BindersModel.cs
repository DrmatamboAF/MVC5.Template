using MvcTemplate.Components.Mvc;
using System;

namespace MvcTemplate.Tests.Objects
{
    public class BindersModel
    {
        [NotTrimmed]
        public String NotTrimmed { get; set; }

        public String Trimmed { get ;set; }
    }
}
