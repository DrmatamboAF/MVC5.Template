using System;

namespace MvcTemplate.Components.Security
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AuthorizeAsAttribute : Attribute
    {
        public String Action { get; private set; }

        public AuthorizeAsAttribute(String action)
        {
            Action = action;
        }
    }
}
