namespace PeachPayments
{
    internal static class Logger
    {
        private static readonly log4net.ILog log =
           log4net.LogManager.GetLogger(typeof(Logger));

        public static void Info(string logData)
        {
            log.Info(logData);
        }

        public static void Error(string logData)
        {
            log.Error(logData);
        }

        public static void Debug(string logData)
        {
            log.Debug(logData);
        }

        public static void Warn(string logData)
        {
            log.Warn(logData);
        }

        public static void Fatal(string logData)
        {
            log.Fatal(logData);
        }
    }
}