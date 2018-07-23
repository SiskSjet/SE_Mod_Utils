using System;
using System.Diagnostics;

namespace Sisk.Utils.Profiler {
    public class ProfilerFrame : IDisposable {
        /// <summary>
        ///     Creates a new instance of <see cref="ProfilerFrame" />.
        /// </summary>
        /// <param name="scope">The scope method for the <see cref="ProfilerFrame" />.</param>
        /// <param name="method">The method for the <see cref="ProfilerFrame" />.</param>
        public ProfilerFrame(string scope, string method) {
            Scope = scope;
            Method = method;
            Name = $"{Scope}.{Method}()";
            Profiler.PushFrame(this);
            Stopwatch = new Stopwatch();
            Stopwatch.Start();
        }

        /// <summary>
        ///     Indicates if the frame start is already logged.
        /// </summary>
        public bool IsFrameStartLogged { get; set; }

        /// <summary>
        ///     The method for the <see cref="ProfilerFrame" />
        /// </summary>
        public string Method { get; }

        public string Name { get; }

        /// <summary>
        ///     The Scope method for the <see cref="ProfilerFrame" />.
        /// </summary>
        public string Scope { get; }

        /// <summary>
        ///     The Stopwatch instance used for this <see cref="ProfilerFrame" />.
        /// </summary>
        public Stopwatch Stopwatch { get; }

        /// <inheritdoc />
        public void Dispose() {
            Stopwatch.Stop();
            Profiler.PopFrame(this);
        }
    }
}