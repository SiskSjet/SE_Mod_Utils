using System;
using System.Collections.Generic;
using Sisk.Utils.Logging.Handler;

namespace Sisk.Utils.Logging {
    public class Logger : IDisposable {
        private static readonly IList<ILogHandler> LogHandlers;
        public static string Origin;
        private readonly string _callingClass;
        private string _callingMethod;

        static Logger() {
            LogHandlers = new List<ILogHandler>();
        }

        private Logger(string callingClass) {
            _callingClass = callingClass;
        }

        public static Level Level { get; set; }

        public static void AddHandler(ILogHandler handler) {
            LogHandlers.Add(handler);
        }

        public static void Flush() {
            foreach (var handler in LogHandlers) {
                handler.Flush();
            }
        }

        public static void RemoveHandler(ILogHandler handler) {
            LogHandlers.Remove(handler);
        }

        public static Logger Scope(string callingClass) {
            return new Logger(callingClass);
        }

        public static Logger Scope<T>() where T : class {
            return Scope(typeof(T).Name);
        }

        public void Debug(string message) {
            Log(Level.Debug, message);
        }

        public void EnterMethod(string methodName) {
            _callingMethod = methodName;
        }

        public void Error(Exception exception, string additionalInformation = null) {
            Log(Level.Error, exception.ToString());
            if (additionalInformation != null) {
                Log(Level.Info, $"Additional information on {exception.Message}: {additionalInformation}");
            }
        }
        public void Error(string message) {
            Log(Level.Error, message);
        }

        public void Info(string message) {
            Log(Level.Info, message);
        }

        public void LeaveMethod() {
            _callingMethod = null;
        }

        public void Warn(string message) {
            Log(Level.Warn, message);
        }

        private void Append(Message message) {
            foreach (var handler in LogHandlers) {
                handler.Enqueue(message);
            }
        }

        private Logger Filter(Level level) {
            return (level & Level) != 0 ? this : null;
        }

        private void Log(Level level, string message) {
            Filter(level)?.Append(new Message(level, message, DateTime.Now, _callingClass, _callingMethod, Origin));
        }

        public void Dispose() {
            foreach (var handler in LogHandlers) {
                handler.Dispose();
            }
        }
    }
}