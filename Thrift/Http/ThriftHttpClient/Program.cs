using System;
using System.Collections.Generic;
using System.Linq;
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

                    int count = 10000;
                    using (Job.StartNew(string.Format("{0} contacts", count)))
                    {
                        contacts = client.addContacts(Enumerable.Range(1, count)
                            .Select(n => new Contact
                            {
                                FirstName = string.Format("FirstName{0}", n),
                                LastName = string.Format("LastName{0}", n),
                                Email = string.Format("email{0}", n)
                            }).ToList());
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
