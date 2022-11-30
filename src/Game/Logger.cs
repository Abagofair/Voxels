namespace Game
{
    internal static class Logger
    {
        private const int MaximumAmount = 1000;
        private static readonly List<LogMessage> _logMessages = new List<LogMessage>(MaximumAmount);

        public static void Info(string message) => AddMessage(message, LogLevel.Info);
        public static void Debug(string message) => AddMessage(message, LogLevel.Debug);

        private static void AddMessage(string message, LogLevel logLevel)
        {
            var logMessage = new LogMessage(
                    DateTimeOffset.UtcNow.TimeOfDay,
                    message,
                    logLevel);

            _logMessages.Add(logMessage);

            Console.WriteLine(logMessage);

            if (_logMessages.Count > MaximumAmount)
            {
                _logMessages.RemoveAt(0);
            }
        }

        public static List<LogMessage> GetLogMessages() => _logMessages;
    }
}
