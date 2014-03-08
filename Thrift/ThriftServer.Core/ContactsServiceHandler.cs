using System.Collections.Generic;
using System.Linq;

namespace ThriftServer
{
    public class ContactsServiceHandler : ContactsService.Iface
    {
        private List<Contact> contacts = new List<Contact>
        {
            new Contact { Id = 1, FirstName = "Tony", LastName = "Stark", Email = "ironman@avenger.com" },
            new Contact { Id = 2, FirstName = "Bruce", LastName = "Banner", Email = "hulk@avenger.com" },
            new Contact { Id = 3, FirstName = "Thor", LastName = "Odinson", Email = "thor@avenger.com" }
        };

        List<Contact> ContactsService.Iface.getContacts()
        {
            return this.contacts;
        }

        List<Contact> ContactsService.Iface.addContacts(List<Contact> contacts)
        {
            int nextId = this.contacts.Select(e => e.Id).Max();
            foreach (var contact in contacts)
            {
                contact.Id = ++nextId;
                this.contacts.Add(contact);
            }
            return contacts;
        }
    }
}
