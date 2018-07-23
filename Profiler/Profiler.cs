using System;
using System.Collections.Generic;
using System.Linq;

namespace Sisk.Utils.Profiler {
    public static class Profiler {
        private static readonly List<ProfilerFrame> Stack = new List<ProfilerFrame>();
        private static readonly Dictionary<string, ProfiledBlock> Totals = new Dictionary<string, ProfiledBlock>();
        private static Action<string> _logger;

        /// <summary>
        ///     The profiler results.
        /// </summary>
        public static IEnumerable<ProfiledBlock> Results => Totals.Values;

        /// <summary>
        ///     Starts measuring until the returned <see cref="ProfilerFrame" /> disposes.
        /// </summary>
        /// <param name="scope">The scope of the profiled code block</param>
        /// <param name="method">The method name of the profiled code block.</param>
        /// <returns>Returns an <see cref="ProfilerFrame" /> which contains the current profiler data.</returns>
        public static ProfilerFrame Measure(string scope, string method) {
            return new ProfilerFrame(scope, method);
        }

        /// <summary>
        ///     Removes a <see cref="ProfilerFrame" /> from the stack.
        /// </summary>
        /// <param name="frame">The <see cref="ProfilerFrame" /> that should be removed from the stack.</param>
        public static void PopFrame(ProfilerFrame frame) {
            _logger?.Invoke($"{new string(' ', Stack.Count * 2 - 2)} {(frame.IsFrameStartLogged ? "<=" : "<>")} {frame.Name}: {frame.Stopwatch.Elapsed.TotalMilliseconds:N6}ms");

            var total = Totals.ContainsKey(frame.Name) ? Totals[frame.Name] : new ProfiledBlock(frame.Scope, frame.Method);
            total.Add(frame);
            Totals[frame.Name] = total;
            Stack.RemoveAt(Stack.Count - 1);
        }

        /// <summary>
        ///     Adds a <see cref="ProfilerFrame" /> to the stack.
        /// </summary>
        /// <param name="frame">The <see cref="ProfilerFrame" /> that should be added to the stack.</param>
        public static void PushFrame(ProfilerFrame frame) {
            if (_logger != null) {
                var last = Stack.LastOrDefault();
                if (last != null && !last.IsFrameStartLogged) {
                    _logger.Invoke($"{new string(' ', Stack.Count * 2 - 2)} => {last.Name}");
                    last.IsFrameStartLogged = true;
                }
            }

            Stack.Add(frame);
        }

        /// <summary>
        ///     Sets an action for logging.
        /// </summary>
        /// <param name="logger">The action used for logging.</param>
        public static void SetLogger(Action<string> logger) {
            _logger = logger;
        }
    }
}