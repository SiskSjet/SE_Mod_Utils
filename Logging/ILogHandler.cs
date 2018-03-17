// ReSharper disable once CheckNamespace

namespace Sisk.Utils.Logging {
    public interface ILogEventHandler {
        void Emit(LogEvent logEvent);
        void Flush();
        bool IsEnabled(LogEventLevel level);
    }
}