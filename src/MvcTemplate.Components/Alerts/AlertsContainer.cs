using System;
using System.Collections.Generic;

namespace MvcTemplate.Components.Alerts
{
    public class AlertsContainer : List<Alert>
    {
        public const Int32 DefaultFadeout = 4000;

        public void Add(AlertType type, String message)
        {
            Add(type, message, DefaultFadeout);
        }
        public void Add(AlertType type, String message, Decimal fadeoutAfter)
        {
            Add(new Alert
            {
                Type = type,
                Message = message,
                FadeoutAfter = fadeoutAfter
            });
        }

        public void AddError(String message)
        {
            AddError(message, 0);
        }
        public void AddError(String message, Decimal fadeoutAfter)
        {
            Add(AlertType.Danger, message, fadeoutAfter);
        }

        public void Merge(AlertsContainer alerts)
        {
            if (alerts == this) return;

            AddRange(alerts);
        }
    }
}
