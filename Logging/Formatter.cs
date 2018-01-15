using System;

namespace Sisk.Utils.Logging {
    public delegate string Formatter(Level level, string text, DateTime dateTime, string callingClass, string callingMethod, string origin);
}