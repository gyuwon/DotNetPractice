using System;
using System.Collections.Generic;
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
        }
    }
}
