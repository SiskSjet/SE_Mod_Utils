namespace Sisk.Utils.Logging {
    public interface ILogEventHandler {
        LogEventLevel Level { get; set; }
        void Close();
        void Emit(LogEvent logEvent);
        void Flush();
        bool IsEnabled(LogEventLevel level);
    }
}