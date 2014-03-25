using System.Collections.Generic;
using System.Threading.Tasks;
using ContactManager.Models;

namespace ContactManager.Services
{
    public interface IContactManagerService
    {
        Task<IEnumerable<Contact>> GetContactsAsync();
        Task<Contact> AddContactAsync(Contact contact);
    }
}
