using System;
using System.Linq;

namespace ContactManager.Models
{
    public interface IRepository : IDisposable
    {
        IQueryable<Contact> Get();
        void Add(Contact contact);
        void SaveChanges();
    }
}
