using System;

namespace Sisk.Utils.Logging {
    [Flags]
    public enum Level : byte {
        Error = 1,
        Warn = 2,
        Info = 4,
        Debug = 8,
        All = Error | Warn | Info | Debug
    }
}