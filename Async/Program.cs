using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Async
{
    class AsyncResult : IAsyncResult
    {
        public object AsyncState { get; set; }
        public WaitHandle AsyncWaitHandle { get; set; }
        public bool CompletedSynchronously { get; set; }
        public bool IsCompleted { get; set; }
    }

    class Program
    {
        const int Delay = 1000;

        static int GetValue()
        {
            Thread.Sleep(Delay);
            return 1;
        }

        static Task<int> GetValueAsync()
        {
            ManualResetEvent mre = new ManualResetEvent(false);
            AsyncResult asyncResult = new AsyncResult
            {
                AsyncState = 1,
                AsyncWaitHandle = mre,
                IsCompleted = false,
                CompletedSynchronously = false
            };
            System.Timers.Timer timer = new System.Timers.Timer(Delay) { AutoReset = false };
            timer.Elapsed += (s, e) =>
            {
                asyncResult.IsCompleted = true;
                asyncResult.CompletedSynchronously = false;
                mre.Set();
                timer.Dispose();
            };
            timer.Start();

            return Task.Factory.FromAsync(asyncResult, ar => (int)ar.AsyncState);
        }

        static void Main(string[] args)
        {
            var s = Enumerable.Range(0, 1000);

            using (Job.StartNew("Async"))
            {
                Task.WaitAll(s.Select(async _ => await GetValueAsync()).ToArray());
            }

            Console.WriteLine();

            using (Job.StartNew("Sync Threads"))
            {
                s.Select(_ =>
                {
                    Thread t = new Thread(() => GetValue());
                    t.Start();
                    return t;
                }).ToList().ForEach(t => t.Join());
            }

            Console.WriteLine();

            using (Job.StartNew("Sync Tasks"))
            {
                Task.WaitAll(s.Select(_ => Task.Factory.StartNew(() => GetValue())).ToArray());
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
