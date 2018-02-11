using System;

namespace Airline.AppLogic.Logging
{
    public interface IServiceLogger : IDisposable
    {
        void Debug(string message);

        void Info(string message);

        void Warning(string message);

        void Error(string message);
    }
}
