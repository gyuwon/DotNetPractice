using System;
using Microsoft.Owin.Hosting;

namespace SelfHostedNancy
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "http://localhost:3000";

            using (WebApp.Start<Startup>(url))
            {
                Console.WriteLine("Running on {0}", url);
                Console.WriteLine("Press enter to exit");
                Console.ReadLine();
            }
        }
    }
}
