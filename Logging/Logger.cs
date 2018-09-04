using System;
using System.Collections.Generic;
using System.Linq;

namespace Sisk.Utils.Logging {
    /// <inheritdoc />
    public class Logger : ILogger {
        private readonly Stack<string> _callingMethods = new Stack<string>();
        private readonly HashSet<Logger> _children = new HashSet<Logger>();
        private readonly HashSet<ILogEventHandler> _logHandlers = new HashSet<ILogEventHandler>();
        private readonly Logger _parent;
        private readonly Type _scope;
        private readonly object _syncObject = new object();

        internal Logger(Type scope, Logger parent = null) {
            _scope = scope;
            _parent = parent;
        }

        /// <inheritdoc />
        ILogger ILogger.ForScope<TScope>() {
            lock (_syncObject) {
                var logger = new Logger(typeof(TScope), this);
                _children.Add(logger);
                return logger;
            }
        }

        /// <inheritdoc />
        public void Info(string message) {
            Write(LogEventLevel.Info, message);
        }

        /// <inheritdoc />
        public void UnRegister(ILogEventHandler eventHandler) {
            lock (_syncObject) {
                _logHandlers.Remove(eventHandler);
            }
        }

        /// <inheritdoc />
        public void Warning(string message) {
            Write(LogEventLevel.Warning, message);
        }

        /// <inheritdoc />
        public void Flush() {
            lock (_syncObject) {
                foreach (var handler in _logHandlers) {
                    handler.Flush();
                }

                foreach (var logger in _children) {
                    logger.Flush();
                }
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
            lock (_syncObject) {
                _callingMethods.Push(method);

                if (LogOnEnterAndLeaveMethods) {
                    Debug("Start");
                }
            }
        }

        /// <inheritdoc />
        public void LeaveMethod() {
            lock (_syncObject) {
                if (LogOnEnterAndLeaveMethods) {
                    Debug("End");
                }

                _callingMethods.Pop();
            }
        }

        /// <inheritdoc />
        public void Register(ILogEventHandler eventHandler) {
            lock (_syncObject) {
                _logHandlers.Add(eventHandler);
            }
        }

        /// <inheritdoc />
        public bool LogOnEnterAndLeaveMethods { get; set; }

        /// <inheritdoc />
        public IDisposable BeginMethod(string methodName) {
            EnterMethod(methodName);
            return new DisposingContext(this);
        }

        /// <inheritdoc />
        public void Close() {
            lock (_syncObject) {
                foreach (var logHandler in _logHandlers) {
                    logHandler.Close();
                }

                foreach (var logger in _children) {
                    logger.Close();
                }
            }
        }

        /// <summary>
        ///     Create a logger that marks log events as being from the specified source type.
        /// </summary>
        /// <typeparam name="TScope">Type generating log messages in the context.</typeparam>
        /// <returns>A logger that will enrich log events as specified.</returns>
        public static ILogger ForScope<TScope>() {
            return new Logger(typeof(TScope));
        }

        private void Dispatch(LogEvent logEvent) {
            lock (_syncObject) {
                foreach (var handler in _logHandlers.Where(x => x.IsEnabled(logEvent.Level))) {
                    handler.Emit(logEvent);
                }

                _parent?.Dispatch(logEvent);
            }
        }

        private void Write(LogEventLevel level, string message) {
            lock (_syncObject) {
                var method = _callingMethods.Any() ? _callingMethods.Peek() : "";
                var logEvent = new LogEvent(DateTime.UtcNow, level, message, _scope, method);
                Dispatch(logEvent);
            }
        }

        private void Write(LogEventLevel level, Exception exception) {
            lock (_syncObject) {
                var method = _callingMethods.Any() ? _callingMethods.Peek() : "";
                var logEvent = new LogEvent(DateTime.UtcNow, level, exception, _scope, method);
                Dispatch(logEvent);
            }
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