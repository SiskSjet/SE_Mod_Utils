using System;

// ReSharper disable once CheckNamespace
namespace Sisk.Utils.Logging {
    public static class Log {
        private static ILogger _instance;

        /// <summary>
        ///     The globally-shared logger.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public static ILogger Default {
            get => _instance ?? (_instance = new Logger(typeof(ILogger)));
            set {
                if (value == null) {
                    throw new ArgumentNullException(nameof(value));
                }

                _instance = value;
            }
        }

        /// <summary>
        ///     Add given method to method stack and remove it on Dispose.
        /// </summary>
        /// <param name="methodName">The method name</param>
        /// <returns></returns>
        public static IDisposable BeginMethod(string methodName) {
            return Default.BeginMethod(methodName);
        }

        /// <summary>
        ///     Write a log event with the <see cref="LogEventLevel.Debug" /> level.
        /// </summary>
        /// <param name="message">Message describing the event.</param>
        public static void Debug(string message) {
            Default.Debug(message);
        }

        /// <summary>
        ///     Add the given method to the method stack.
        /// </summary>
        /// <param name="methodName">The method name</param>
        public static void EnterMethod(string methodName) {
            Default.EnterMethod(methodName);
        }

        /// <summary>
        ///     Write a log event with the <see cref="LogEventLevel.Error" /> level.
        /// </summary>
        /// <param name="message">Message describing the event.</param>
        public static void Error(string message) {
            Default.Error(message);
        }

        /// <summary>
        ///     Write a log event with the <see cref="LogEventLevel.Error" /> level.
        /// </summary>
        /// <param name="exception">Exception describing the event.</param>
        public static void Error(Exception exception) {
            Default.Error(exception);
        }

        /// <summary>
        ///     Clears buffers for this stream and causes any buffered data to be written to the file.
        /// </summary>
        public static void Flush() {
            Default.Flush();
        }

        /// <summary>
        ///     Create a logger that marks log events as being from the specified source type
        /// </summary>
        /// <typeparam name="TScope">Type generating log messages in the context.</typeparam>
        /// <returns>A logger that will enrich log events as specified.</returns>
        public static ILogger ForScope<TScope>() {
            return Default.ForScope<TScope>();
        }

        /// <summary>
        ///     Write a log event with the <see cref="LogEventLevel.Info" /> level.
        /// </summary>
        /// <param name="message">Message describing the event.</param>
        public static void Info(string message) {
            Default.Info(message);
        }

        /// <summary>
        ///     Remove the last method from method stack.
        /// </summary>
        public static void LeaveMethod() {
            Default.LeaveMethod();
        }

        /// <summary>
        ///     Register a log event handler.
        /// </summary>
        /// <param name="eventHandler"></param>
        public static void Register(ILogEventHandler eventHandler) {
            Default.Register(eventHandler);
        }

        /// <summary>
        ///     Remove a log event handler.
        /// </summary>
        /// <param name="eventHandler"></param>
        public static void UnRegister(ILogEventHandler eventHandler) {
            Default.UnRegister(eventHandler);
        }

        /// <summary>
        ///     Write a log event with the <see cref="LogEventLevel.Warning" /> level.
        /// </summary>
        /// <param name="message">Message describing the event.</param>
        public static void Warning(string message) {
            Default.Warning(message);
        }
    }
}