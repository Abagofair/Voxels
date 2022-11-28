namespace Game
{
    internal struct LogMessage
    {
        public TimeSpan timeSpan;
        public string message;
        public LogLevel level;

        public LogMessage(
            TimeSpan timeSpan,
            string message,
            LogLevel level)
        {
            this.timeSpan = timeSpan;
            this.message = message;
            this.level = level;
        }

        public override string ToString()
        {
            var time = timeSpan.ToString("hh\\:mm\\:ss");
            var logLevel = level.ToString().ToUpper();

            return $"[{time} - {logLevel}] {message}";
        }
    }
}
