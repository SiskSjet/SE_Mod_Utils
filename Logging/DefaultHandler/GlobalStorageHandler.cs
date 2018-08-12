using System.Collections.Generic;
using System.IO;
using Sandbox.ModAPI;

namespace Sisk.Utils.Logging.DefaultHandler {
    public sealed class GlobalStorageHandler : LogEventHandler {
        private readonly int _bufferSize;
        private readonly Queue<LogEvent> _cache = new Queue<LogEvent>();
        private readonly string _fileName;
        private readonly Formatter _formatter;
        private readonly object _syncObject = new object();
        private TextWriter _logWriter;
        public GlobalStorageHandler(string fileName, LogEventLevel level = LogEventLevel.All) : this(fileName, LogEvent.DefaultFormatter, level) { }

        public GlobalStorageHandler(string fileName, Formatter formatter, LogEventLevel level = LogEventLevel.All, int bufferSize = 50) : base(level) {
            _fileName = fileName;
            _formatter = formatter;
            _bufferSize = bufferSize;
        }

        /// <inheritdoc />
        public override void Close() {
            if (_logWriter != null) {
                _logWriter.Flush();
                _logWriter.Close();
                _logWriter.Dispose();
                _logWriter = null;
            }
        }

        /// <inheritdoc />
        public override void Emit(LogEvent logEvent) {
            lock (_syncObject) {
                _cache.Enqueue(logEvent);

                if (_cache.Count >= _bufferSize) {
                    Flush();
                }
            }
        }

        /// <inheritdoc />
        public override void Flush() {
            if (_logWriter == null && MyAPIGateway.Utilities != null) {
                _logWriter = MyAPIGateway.Utilities.WriteFileInGlobalStorage(_fileName);
            }

            if (_logWriter != null) {
                lock (_syncObject) {
                    while (_cache.Count > 0) {
                        var logEvent = _cache.Dequeue();
                        _logWriter.WriteLine(logEvent.RenderMessage(_formatter));
                    }
                }

                _logWriter.Flush();
            }
        }
    }
}