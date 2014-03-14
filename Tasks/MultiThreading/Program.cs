using System;
using System.Threading;

namespace MultiThreading
{
    class Program
    {
        static void Main(string[] args)
        {
            var count = 1000;

            var t = new Thread(() => Write('+', count));
            t.Start();

            Write('-', count);

            t.Join();
        }

        static void Write(char c, int count)
        {
            for (int i = 0; i < count; i++)
            {
                Console.Write(c);
            }
        }
    }
}
