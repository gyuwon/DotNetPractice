using System;
using System.Collections.Generic;
using Thrift;
using Thrift.Protocol;
using Thrift.Transport;

namespace ThriftHttpClient
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                TTransport transport = new THttpClient(new Uri("http://localhost:3000"));
                TProtocol protocol = new TBinaryProtocol(transport);
                ContactsService.Client client = new ContactsService.Client(protocol);
                transport.Open();
                try
                {
                    var contacts = client.getContacts();
                    foreach (var e in contacts)
                    {
                        Console.WriteLine(e);
                    }

                    contacts = client.addContacts(new List<Contact>
                    {
                        new Contact { FirstName = "Bruce", LastName = "Wayne", Email = "batman@justiceleague.com" },
                        new Contact { FirstName = "Clark", LastName = "Kent", Email = "superman@justiceleague.com" }
                    });
                    foreach (var e in contacts)
                    {
                        Console.WriteLine(e);
                    }
                }
                finally
                {
                    transport.Close();
                }
            }
            catch (TApplicationException exception)
            {
                Console.WriteLine(exception.StackTrace);
            }
        }
    }
}
