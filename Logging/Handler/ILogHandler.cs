using System;

namespace Sisk.Utils.Logging.Handler {
    public interface ILogHandler : IDisposable {
        void Enqueue(Message message);
        void Flush();
    }
}