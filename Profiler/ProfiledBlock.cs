namespace Sisk.Utils.Profiler {
    /// <summary>
    ///     A class to store profiler data.
    /// </summary>
    public class ProfiledBlock {
        /// <summary>
        ///     Creates a new instance of <see cref="ProfiledBlock" />.
        /// </summary>
        /// <param name="scope">The scope method for the profiled <see cref="ProfiledBlock" />.</param>
        /// <param name="method">The method method for the profiled <see cref="ProfiledBlock" />.</param>
        public ProfiledBlock(string scope, string method) {
            Scope = scope;
            Method = method;
            Min = double.MaxValue;
        }

        /// <summary>
        ///     The average execution for this <see cref="ProfiledBlock" /> time.
        /// </summary>
        public double Avg { get; private set; }

        /// <summary>
        ///     Amount of executions of this <see cref="ProfiledBlock" />.
        /// </summary>
        public int Executions { get; private set; }

        /// <summary>
        ///     Max execution time for this <see cref="ProfiledBlock" /> in milliseconds.
        /// </summary>
        public double Max { get; private set; }

        /// <summary>
        ///     The method method for the profiled <see cref="ProfiledBlock" />.
        /// </summary>
        public string Method { get; }

        /// <summary>
        ///     Minimun execution time for this <see cref="ProfiledBlock" /> in milliseconds.
        /// </summary>
        public double Min { get; private set; }

        /// <summary>
        ///     The Scope method for the profiled <see cref="ProfiledBlock" />.
        /// </summary>
        public string Scope { get; }

        /// <summary>
        ///     Total execution time for this <see cref="ProfiledBlock" /> in milliseconds elapsed.
        /// </summary>
        public double Total { get; private set; }

        private static string Format(int num) {
            if (num >= 1000000) {
                return $"{num / 1000000,7:N1}M";
            }

            if (num >= 1000) {
                return $"{num / 1000,7:N1}K";
            }

            return $"{num,8}";
        }

        /// <inheritdoc />
        public override string ToString() {
            return $"avg {Avg:N6}ms, min {Min:N6}ms, max {Max:N6}ms, total {Total:N2}ms :{Scope}.{Method}()=> {Format(Executions)} executions";
        }

        /// <summary>
        ///     Adds a new <see cref="ProfilerFrame" /> to this <see cref="ProfiledBlock" />.
        /// </summary>
        /// <param name="frame">The <see cref="ProfilerFrame" /> that gets added.</param>
        public void Add(ProfilerFrame frame) {
            var ms = frame.Stopwatch.Elapsed.TotalMilliseconds;
            Executions++;
            Total += ms;

            if (ms < Min) {
                Min = ms;
            }

            if (ms > Max) {
                Max = ms;
            }

            Avg = Total / Executions;
        }
    }
}