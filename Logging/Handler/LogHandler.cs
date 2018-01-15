namespace Sisk.Utils.Logging.Handler {
    public abstract class LogHandler : ILogHandler {
        protected LogHandler(Formatter formatter) {
            Formatter = formatter;
        }

        protected Formatter Formatter { get; }
        public abstract void Enqueue(Message message);
        public abstract void Flush();

        public abstract void Dispose();
    }
}