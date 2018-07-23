using System;

namespace Sisk.Utils.Logging {
    public delegate string Formatter(LogEventLevel level, string message, DateTime timestamp, Type scope, string method);

    public class LogEvent {
        public static readonly Formatter DefaultFormatter = (level, message, timestamp, scope, method) => $"[{level}] {message}";

        public LogEvent(DateTime timestamp, LogEventLevel level, Exception exception, Type scope, string method) : this(timestamp, level, exception.Message, scope, method) {
            Exception = exception;
        }

        public LogEvent(DateTime timestamp, LogEventLevel level, string message, Type scope, string method) {
            Timestamp = timestamp;
            Level = level;
            Message = message;
            Scope = scope;
            Method = method;
        }

        /// <summary>
        ///     The Exception if any.
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        ///     The level of the event.
        /// </summary>
        public LogEventLevel Level { get; }

        /// <summary>
        ///     The event message.
        /// </summary>
        public string Message { get; }

        /// <summary>
        ///     The method this event emit.
        /// </summary>
        public string Method { get; }

        /// <summary>
        ///     The Scope in which this event happens.
        /// </summary>
        public Type Scope { get; }

        /// <summary>
        ///     The time at which the event occurred.
        /// </summary>
        public DateTime Timestamp { get; }

        /// <summary>
        ///     Render the message with the specified formatter.
        /// </summary>
        /// <param name="formatter">The formatter used to render the message.</param>
        /// <returns></returns>
        public string RenderMessage(Formatter formatter) {
            return formatter(Level, Message, Timestamp.ToLocalTime(), Scope, Method);
        }
    }
}