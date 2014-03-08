using System;
using Thrift.Server;
using Thrift.Transport;

namespace ThriftServer
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var handler = new ContactsServiceHandler();
                var processor = new ContactsService.Processor(handler);
                var serverTransport = new TServerSocket(3030);
                var server = new TSimpleServer(processor, serverTransport);

                Console.WriteLine("Starting the server...");
                server.Serve();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.StackTrace);
            }
            Console.WriteLine("done.");
        }
    }
}
