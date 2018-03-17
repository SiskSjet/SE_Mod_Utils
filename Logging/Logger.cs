using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace Sisk.Utils.Logging {
    /// <inheritdoc />
    public class Logger : ILogger {
        private readonly Stack<string> _callingMethods = new Stack<string>();
        private readonly HashSet<Logger> _children = new HashSet<Logger>();
        private readonly HashSet<ILogEventHandler> _logHandlers = new HashSet<ILogEventHandler>();
        private readonly Logger _parent;
        private readonly Type _scope;

        internal Logger(Type scope, Logger parent = null) {
            _scope = scope;
            _parent = parent;
        }

        /// <inheritdoc />
        public ILogger ForScope<TScope>() {
            var logger = new Logger(typeof(TScope), this);
            _children.Add(logger);
            return logger;
        }

        /// <inheritdoc />
        public void Info(string message) {
            Write(LogEventLevel.Info, message);
        }

        /// <inheritdoc />
        public void UnRegister(ILogEventHandler eventHandler) {
            _logHandlers.Remove(eventHandler);
        }

        /// <inheritdoc />
        public void Warning(string message) {
            Write(LogEventLevel.Warning, message);
        }

        /// <inheritdoc />
        public void Flush() {
            foreach (var handler in _logHandlers) {
                handler.Flush();
            }

            foreach (var logger in _children) {
                logger.Flush();
            }
        }

        /// <inheritdoc />
        public void Error(string message) {
            Write(LogEventLevel.Error, message);
        }

        /// <inheritdoc />
        public void Error(Exception exception) {
            Write(LogEventLevel.Error, exception);
        }

        /// <inheritdoc />
        public void Debug(string message) {
            Write(LogEventLevel.Debug, message);
        }

        /// <inheritdoc />
        public void EnterMethod(string method) {
            _callingMethods.Push(method);
        }

        /// <inheritdoc />
        public void LeaveMethod() {
            _callingMethods.Pop();
        }

        /// <inheritdoc />
        public void Register(ILogEventHandler eventHandler) {
            _logHandlers.Add(eventHandler);
        }

        /// <inheritdoc />
        public IDisposable BeginMethod(string methodName) {
            EnterMethod(methodName);
            return new DisposingContext(this);
        }

        private void Dispatch(LogEvent logEvent) {
            foreach (var handler in _logHandlers.Where(x => x.IsEnabled(logEvent.Level))) {
                handler.Emit(logEvent);
            }

            _parent?.Dispatch(logEvent);
        }

        private void Write(LogEventLevel level, string message) {
            var method = _callingMethods.Any() ? _callingMethods.Peek() : "";
            var logEvent = new LogEvent(DateTime.Now, level, message, _scope, method);
            Dispatch(logEvent);
        }

        private void Write(LogEventLevel level, Exception exception) {
            var method = _callingMethods.Any() ? _callingMethods.Peek() : "";
            var logEvent = new LogEvent(DateTime.Now, level, exception, _scope, method);
            Dispatch(logEvent);
        }

        public class DisposingContext : IDisposable {
            private readonly Logger _logger;

            public DisposingContext(Logger logger) {
                _logger = logger;
            }

            public void Dispose() {
                _logger.LeaveMethod();
            }
        }
    }
}