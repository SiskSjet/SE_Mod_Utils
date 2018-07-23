namespace Sisk.Utils.Logging {
    public interface ILogEventHandler {
        /// <summary>
        ///     Shows the <see cref="LogEventLevel" /> for this <see cref="ILogEventHandler" />.
        /// </summary>
        LogEventLevel Level { get; set; }

        /// <summary>
        ///     Closes the current <see cref="ILogEventHandler" /> and releases any system resources associated with the
        ///     <see cref="ILogEventHandler" />.
        /// </summary>
        void Close();

        /// <summary>
        ///     Send out an <see cref="LogEvent" /> to the <see cref="ILogEventHandler" />.
        /// </summary>
        /// <param name="logEvent"></param>
        void Emit(LogEvent logEvent);

        /// <summary>
        ///     Clears all buffers for the current writer and causes any buffered data to be written to the underlying device.
        /// </summary>
        void Flush();

        /// <summary>
        ///     Check if this <see cref="ILogEventHandler" /> is enabled for the given <see cref="LogEventLevel" />.
        /// </summary>
        /// <param name="level">The <see cref="LogEventLevel" /> that should be check.</param>
        /// <returns>Returns true if this <see cref="ILogEventHandler" /> is enabled for the given <see cref="LogEventLevel" />.</returns>
        bool IsEnabled(LogEventLevel level);
    }
}