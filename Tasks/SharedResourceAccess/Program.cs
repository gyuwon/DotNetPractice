using System;
using System.Linq;
using System.Threading;

namespace SharedResourceAccess
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (Job.StartNew("Without synchronization"))
            {
                int count = 0;
                Execute(10, () =>
                {
                    for (int i = 0; i < 100000; i++)
                    {
                        count++;
                    }
                });
                Console.WriteLine("count: {0}", count);
            }

            Console.WriteLine();

            using (Job.StartNew("Synchronized with Interlocked class"))
            {
                int count = 0;
                Execute(10, () =>
                {
                    for (int i = 0; i < 100000; i++)
                    {
                        Interlocked.Increment(ref count);
                    }
                });
                Console.WriteLine("count: {0}", count);
            }

            Console.WriteLine();

            using (Job.StartNew("Synchronized with lock statement(Monitor class)"))
            {
                object syncRoot = new object();
                int count = 0;
                Execute(10, () =>
                {
                    for (int i = 0; i < 100000; i++)
                    {
                        lock (syncRoot)
                        {
                            count++;
                        }
                    }
                });
                Console.WriteLine("count: {0}", count);
            }
        }

        private static void Execute(int threadCount, ThreadStart start)
        {
            var threads = Enumerable.Range(0, threadCount).Select(_ => new Thread(start)).ToList();
            foreach (var t in threads)
            {
                t.Start();
            }
            foreach (var t in threads)
            {
                t.Join();
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
