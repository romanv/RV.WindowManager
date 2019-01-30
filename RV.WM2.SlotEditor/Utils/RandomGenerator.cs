namespace RV.WM2.SlotEditor.Utils
{
    using System;
    using System.Threading;

    public static class RandomGenerator
    {
        private static readonly ThreadLocal<Random> ThreadLocal =
            new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref seed)));

        private static int seed;

        static RandomGenerator()
        {
            seed = Environment.TickCount;
        }

        public static Random Instance => ThreadLocal.Value;
    }
}
