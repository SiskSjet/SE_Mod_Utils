using System;

namespace Sisk.Utils.Logging {
    public interface ILogger {
        /// <summary>
        ///     Option to set if ILogger should create a log entry on <see cref="BeginMethod" />, <see cref="EnterMethod" /> and
        ///     <see cref="LeaveMethod" /> methods.
        /// </summary>
        bool LogOnEnterAndLeaveMethods { get; set; }

        /// <summary>
        ///     Add given method to method stack and remove it on Dispose.
        /// </summary>
        /// <param name="methodName">The method name</param>
        /// <returns></returns>
        IDisposable BeginMethod(string methodName);

        /// <summary>
        ///     Close all <see cref="ILogEventHandler" />.
        /// </summary>
        void Close();

        /// <summary>
        ///     Write a log event with the <see cref="LogEventLevel.Debug" /> level.
        /// </summary>
        /// <param name="message">Message describing the event.</param>
        void Debug(string message);

        /// <summary>
        ///     Add the given method to the method stack.
        /// </summary>
        /// <param name="methodName">The method name</param>
        void EnterMethod(string methodName);

        /// <summary>
        ///     Write a log event with the <see cref="LogEventLevel.Error" /> level.
        /// </summary>
        /// <param name="message">Message describing the event.</param>
        void Error(string message);

        /// <summary>
        ///     Write a log event with the <see cref="LogEventLevel.Error" /> level.
        /// </summary>
        /// <param name="exception">Exception describing the event.</param>
        void Error(Exception exception);

        /// <summary>
        ///     Clears buffers for this stream and causes any buffered data to be written to the file.
        /// </summary>
        void Flush();

        /// <summary>
        ///     Create a logger that marks log events as being from the specified source type
        /// </summary>
        /// <typeparam name="TScope">Type generating log messages in the context.</typeparam>
        /// <returns>A logger that will enrich log events as specified.</returns>
        ILogger ForScope<TScope>();

        /// <summary>
        ///     Write a log event with the <see cref="LogEventLevel.Info" /> level.
        /// </summary>
        /// <param name="message">Message describing the event.</param>
        void Info(string message);

        /// <summary>
        ///     Remove the last method from method stack.
        /// </summary>
        void LeaveMethod();

        /// <summary>
        ///     Register a log event handler.
        /// </summary>
        /// <param name="eventHandler"></param>
        void Register(ILogEventHandler eventHandler);

        /// <summary>
        ///     Remove a log event handler.
        /// </summary>
        /// <param name="eventHandler"></param>
        void UnRegister(ILogEventHandler eventHandler);

        /// <summary>
        ///     Write a log event with the <see cref="LogEventLevel.Warning" /> level.
        /// </summary>
        /// <param name="message">Message describing the event.</param>
        void Warning(string message);
    }
}