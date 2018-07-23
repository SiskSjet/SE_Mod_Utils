namespace Sisk.Utils.Logging {
    /// <inheritdoc />
    public abstract class LogEventHandler : ILogEventHandler {
        /// <inheritdoc />
        protected LogEventHandler(LogEventLevel level) {
            Level = level;
        }

        /// <inheritdoc />
        public LogEventLevel Level { get; set; }

        /// <inheritdoc />
        public abstract void Emit(LogEvent logEvent);

        /// <inheritdoc />
        public abstract void Flush();

        /// <inheritdoc />
        public bool IsEnabled(LogEventLevel level) {
            return (level & Level) != 0;
        }

        /// <inheritdoc />
        public abstract void Close();
    }
}