using System;
using System.Linq;
using System.Threading;

namespace SharedResourceAccess
{
    class Program
    {
        static void Main(string[] args)
        {
            // Without Synchronization
            int count = 0;
            Execute(10, () =>
            {
                for (int i = 0; i < 100000; i++)
                {
                    count++;
                }
            });
            Console.WriteLine("count: {0}", count);

            // With Synchronization
            count = 0;
            Execute(10, () =>
            {
                for (int i = 0; i < 100000; i++)
                {
                    Interlocked.Increment(ref count);
                }
            });
            Console.WriteLine("count: {0}", count);
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
}
