using System;

namespace Sisk.Utils.Logging {
    public class Message {
        public static readonly Formatter DefaultFormatter = (level, text, dateTime, callingClass, callingMethod, origin) => $"[{level}] {text}";

        public Message(Level level, string text, DateTime dateTime, string callingClass, string callingMethod, string origin) {
            Level = level;
            DateTime = dateTime;
            CallingClass = callingClass;
            CallingMethod = callingMethod;
            Origin = origin;
            Text = text;
        }

        public string CallingClass { get; }
        public string CallingMethod { get; }
        public DateTime DateTime { get; }
        public Level Level { get; }
        public string Origin { get; }
        public string Text { get; }

        public override string ToString() {
            return ApplyFormat(DefaultFormatter);
        }

        public string ApplyFormat(Formatter formatter) {
            return formatter(Level, Text, DateTime, CallingClass, CallingMethod, Origin);
        }
    }
}