using System;

namespace Sisk.Utils.Logging {
    [Flags]
    public enum LogEventLevel {
        None = 0,

        /// <summary>
        ///     The lifeblood of operational intelligence - things happen.
        /// </summary>
        Info = 1,

        /// <summary>
        ///     Service is degraded or endangered.
        /// </summary>
        Warning = 2,

        /// <summary>
        ///     Functionality is unavailable, invariants are broken or data is lost.
        /// </summary>
        Error = 4,

        /// <summary>
        ///     Internal system events that aren't necessarily observable from the outside.
        /// </summary>
        Debug = 8,
        All = Info | Warning | Error | Debug
    }
}