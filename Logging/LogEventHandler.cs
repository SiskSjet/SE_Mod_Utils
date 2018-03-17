namespace Sisk.Utils.Logging {
    /// <inheritdoc />
    public abstract class LogEventHandler : ILogEventHandler {
        /// <inheritdoc />
        protected LogEventHandler(LogEventLevel level) {
            Level = level;
        }

        /// <summary>
        ///     Shows the <see cref="LogEventLevel" /> for this <see cref="LogEventHandler" />.
        /// </summary>
        public LogEventLevel Level { get; }

        /// <inheritdoc />
        public abstract void Emit(LogEvent logEvent);

        /// <inheritdoc />
        public abstract void Flush();

        /// <inheritdoc />
        public bool IsEnabled(LogEventLevel level) {
            return (level & Level) != 0;
        }
    }
}