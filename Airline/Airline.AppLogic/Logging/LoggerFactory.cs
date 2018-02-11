namespace Airline.AppLogic.Logging
{
    public static class LoggerFactory
    {
        public static IServiceLogger GetServiceLogger(string name)
        {
            return new ServiceLogger(name);
        }
    }
}
