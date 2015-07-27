using Newtonsoft.Json;
using System;
using System.Data.Entity.Infrastructure;
using System.IO;

namespace MvcTemplate.Data.Logging
{
    public class LoggableProperty
    {
        private String PropertyName { get; set; }
        private Object CurrentValue { get; set; }
        private Object OriginalValue { get; set; }
        public Boolean IsModified { get; private set; }

        public LoggableProperty(DbPropertyEntry entry, Object originalValue)
        {
            PropertyName = entry.Name;
            OriginalValue = originalValue;
            CurrentValue = entry.CurrentValue;
            IsModified = entry.IsModified && !Equals(OriginalValue, CurrentValue);
        }

        public override String ToString()
        {
            if (IsModified)
                return PropertyName + ": " + Format(OriginalValue) + " => " + Format(CurrentValue);

            return PropertyName + ": " + Format(OriginalValue);
        }

        private String Format(Object value)
        {
            if (value == null)
                return "null";

            if (value is DateTime?)
                return "\"" + ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss") + "\"";

            using (StringWriter stringWiter = new StringWriter())
            {
                using (JsonTextWriter jsonWriter = new JsonTextWriter(stringWiter))
                {
                    jsonWriter.WriteValue(value);

                    return stringWiter.ToString();
                }
            }
        }
    }
}
