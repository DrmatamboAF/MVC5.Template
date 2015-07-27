using System;

namespace MvcTemplate.Components.Logging
{
    public interface ILogger : IDisposable
    {
        void Log(String message);
        void Log(String accountId, String message);
    }
}
