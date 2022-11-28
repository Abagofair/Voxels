namespace Game
{
    internal static class Logger
    {
        private const int MaximumAmount = 1000;
        private static readonly List<LogMessage> _logMessages = new List<LogMessage>(MaximumAmount);

        public static void Debug(string message)
        {
            var logMessage = new LogMessage(
                    DateTimeOffset.UtcNow.TimeOfDay,
                    message,
                    LogLevel.Debug);

            _logMessages.Add(logMessage);

            Console.WriteLine(logMessage);
        }

        public static void AddMessage(LogMessage message)
        {
            _logMessages.Add(message);

            if (_logMessages.Count > MaximumAmount)
            {
                _logMessages.RemoveAt(0);
            }
        }

        public static List<LogMessage> GetLogMessages() => _logMessages;
    }
}
