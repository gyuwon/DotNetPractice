using System;

namespace Batch
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Input a number: ");
            string input = Console.ReadLine();
            double n;
            if (double.TryParse(input, out n))
            {
                double square = n * n;
                Console.WriteLine("Input: {0}", n);
                Console.WriteLine("Square: {0}", square);
            }
            else
            {
                Console.WriteLine("Wrong input");
            }
            Console.WriteLine("Press [enter] to quit...");
            Console.ReadLine();
        }
    }
}
