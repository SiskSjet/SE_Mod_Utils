namespace Sisk.Utils.Logging {
    public interface ILogEventHandler {
        void Close();
        void Emit(LogEvent logEvent);
        void Flush();
        bool IsEnabled(LogEventLevel level);
    }
}