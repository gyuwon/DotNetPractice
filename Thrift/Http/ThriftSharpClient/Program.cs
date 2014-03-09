using System;
using System.Collections.Generic;
using System.Linq;
using ThriftSharp;

namespace ThriftSharpClient
{
    [ThriftStruct("Contact")]
    public class Contact
    {
        [ThriftField(1, false, "id")]
        public int Id { get; set; }
        [ThriftField(2, true, "firstName")]
        public string FirstName { get; set; }
        [ThriftField(3, true, "lastName")]
        public string LastName { get; set; }
        [ThriftField(4, true, "email")]
        public string Email { get; set; }

        public override string ToString()
        {
            return string.Format("Contact(Id: {0}, FirstName: {1}, LastName: {2}, Email: {3})", this.Id, this.FirstName, this.LastName, this.Email);
        }
    }

    [ThriftService("ContactsService")]
    public interface IContactsService
    {
        [ThriftMethod("getContacts")]
        List<Contact> GetContacts();
        [ThriftMethod("addContacts")]
        List<Contact> AddContacts([ThriftParameter(1, "contacts")]List<Contact> contacts);
    }
    
    class Program
    {
        static void Main(string[] args)
        {
            var communication = ThriftCommunication.Binary().OverHttp("http://localhost:3000/");
            var service = ThriftProxy.Create<IContactsService>(communication);

            var contacts = service.GetContacts();
            foreach (var e in contacts)
            {
                Console.WriteLine(e);
            }

            contacts = service.AddContacts(new List<Contact>
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
                contacts = service.AddContacts(Enumerable.Range(1, count)
                    .Select(n => new Contact
                    {
                        FirstName = string.Format("FirstName{0}", n),
                        LastName = string.Format("LastName{0}", n),
                        Email = string.Format("email{0}", n)
                    }).ToList());
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
