using System;
using System.Linq;
using System.Threading;

namespace ForAll
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Main Thread: {0}", Thread.CurrentThread.ManagedThreadId);
            Console.WriteLine();

            var source = Enumerable.Range(1, 10);

            var query = from e in source.AsParallel()
                        select e;

            using (Job.StartNew("foreach"))
            {
                foreach (var e in query)
                {
                    Console.WriteLine("e: {0}, Thread: {1}", e, Thread.CurrentThread.ManagedThreadId);
                }
            }

            Console.WriteLine();

            using (Job.StartNew("ForAll"))
            {
                query.ForAll(e => Console.WriteLine("e: {0}, Thread: {1}", e, Thread.CurrentThread.ManagedThreadId));
            }
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
