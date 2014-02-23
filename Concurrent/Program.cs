using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Concurrent
{
    class Program
    {
        static void Main(string[] args)
        {
            int degreeOfParallelism = 50;

            var payloads = new List<int>(Enumerable.Range(0, 1000).Select(i => i));
            Action<int> proc = _ => Thread.Sleep(10);

            int workerThreads, completionPortThreads;
            System.Threading.ThreadPool.GetMinThreads(out workerThreads, out completionPortThreads);
            System.Threading.ThreadPool.SetMinThreads(degreeOfParallelism, completionPortThreads);

            var methods = new Action<IEnumerable<int>, Action<int>, int>[] { Threads, Tasks, PLinq };
            foreach (var method in methods)
            {
                object syncRoot = new object();
                int concurrency = 0;
                int maxConcurrency = 0;
                using (Job.StartNew(method.Method.Name))
                {
                    method(payloads, payload =>
                    {
                        lock (syncRoot) { concurrency++; maxConcurrency = Math.Max(maxConcurrency, concurrency); }
                        proc(payload);
                        lock (syncRoot) { concurrency--; }
                    }, degreeOfParallelism);
                    Console.WriteLine("Max Concurrency: {0}", maxConcurrency);
                }
                Console.WriteLine();
            }
        }

        private static void Threads<T>(IEnumerable<T> payloads, Action<T> proc, int degreeOfParallelism)
        {
            ConcurrentQueue<T> queue = new ConcurrentQueue<T>(payloads);
            Enumerable.Range(0, degreeOfParallelism).Select(_ =>
            {
                Thread t = new Thread(() =>
                {
                    while (true)
                    {
                        T payload;
                        if (queue.TryDequeue(out payload) == false)
                            break;
                        proc(payload);
                    }
                });
                t.Start();
                return t;
            }).ToList().ForEach(t => t.Join());
        }

        private static void Tasks<T>(IEnumerable<T> payloads, Action<T> proc, int degreeOfParallelism)
        {
            ConcurrentQueue<T> queue = new ConcurrentQueue<T>(payloads);
            Task.WaitAll(Enumerable.Range(0, degreeOfParallelism).Select(_ =>
                Task.Run(() =>
                {
                    while (true)
                    {
                        T payload;
                        if (queue.TryDequeue(out payload) == false)
                            break;
                        proc(payload);
                    }
                })
            ).ToArray());
        }

        private static void PLinq<T>(IEnumerable<T> payloads, Action<T> proc, int degreeOfParallelism)
        {
            payloads
                .AsParallel()
                .WithDegreeOfParallelism(degreeOfParallelism)
                .ForAll(proc);
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
