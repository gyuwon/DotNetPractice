using System;
using System.Linq;
using System.Threading;

namespace ParallelismPerformance
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var source = Enumerable.Range(0, 100).Select<int, double>(x => x);

            using (Job.StartNew("Sequential Square"))
            {
                var query = from e in source
                            select Square(e);
                double ss = 0.0;
                foreach (var s in query)
                {
                    ss += s;
                }
                Console.WriteLine("Sum of Squares: {0}", ss);
            }

            Console.WriteLine();

            using (Job.StartNew("Parallel Square"))
            {
                var query = from e in source.AsParallel()
                            select Square(e);
                double ss = 0.0;
                foreach (var s in query)
                {
                    ss += s;
                }
                Console.WriteLine("Sum of Squares: {0}", ss);
            }

            Console.WriteLine();

            using (Job.StartNew("Sequential SquareSlow"))
            {
                var query = from e in source
                            select SquareSlow(e);
                double ss = 0.0;
                foreach (var s in query)
                {
                    ss += s;
                }
                Console.WriteLine("Sum of Squares: {0}", ss);
            }

            Console.WriteLine();

            using (Job.StartNew("Parallel SquareSlow"))
            {
                var query = from e in source.AsParallel()
                            select SquareSlow(e);
                double ss = 0.0;
                foreach (var s in query)
                {
                    ss += s;
                }
                Console.WriteLine("Sum of Squares: {0}", ss);
            }
        }

        static double Square(double x)
        {
            return x * x;
        }

        static double SquareSlow(double x)
        {
            Thread.Sleep(10);
            return x * x;
        }
    }

    class Job : System.IDisposable
    {
        public static Job StartNew(string name)
        {
            return new Job(name);
        }

        private string _name;
        private System.Diagnostics.Stopwatch _stopwatch;

        private Job(string name)
        {
            this._name = name;
            this._stopwatch = System.Diagnostics.Stopwatch.StartNew();
            System.Console.WriteLine("[{0} started]", this._name);
        }

        public void Dispose()
        {
            this._stopwatch.Stop();
            System.Console.WriteLine("[{0} finished] {1}ms elapsed", this._name, this._stopwatch.ElapsedMilliseconds);
        }
    }
}
