using System.Collections.Generic;
using System.IO;
using Sandbox.ModAPI;

namespace Sisk.Utils.Logging.Handler {
    public sealed class WorldStorageHandler : LogHandler {
        private readonly string _fileName;
        private TextWriter _logWriter;
        private readonly Queue<Message> _cache;
        public WorldStorageHandler(string fileName) : this(fileName, Message.DefaultFormatter) { }

        public WorldStorageHandler(string fileName, Formatter formatter) : base(formatter) {
            _fileName = fileName;
            _cache = new Queue<Message>();
        }

        public override void Dispose() {
            if (_logWriter != null) {
                _logWriter.Flush();
                _logWriter.Close();
                _logWriter.Dispose();
                _logWriter = null;
            }
        }

        public override void Enqueue(Message message) {
            lock (_cache) {
                _cache.Enqueue(message);

                Flush();
            }
        }

        public override void Flush() {
            if (_logWriter == null && MyAPIGateway.Utilities != null) {
                _logWriter = MyAPIGateway.Utilities.WriteFileInWorldStorage(_fileName, typeof(Logger));
            }

            if (_logWriter != null) {
                lock (_cache) {
                    while (_cache.Count > 0) {
                        var msg = _cache.Dequeue();
                        _logWriter.WriteLine(msg.ApplyFormat(Formatter));
                    }
                }

                _logWriter.Flush();
            }
        }
    }
}