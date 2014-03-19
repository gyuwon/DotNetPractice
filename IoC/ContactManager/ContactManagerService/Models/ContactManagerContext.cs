using System.Data.Entity;
using System.Linq;

namespace ContactManager.Models
{
    public class ContactManagerContext : DbContext, IRepository
    {
        public ContactManagerContext()
        {
            this.Database.Log += System.Console.WriteLine;
        }

        public DbSet<Contact> Contacts { get; set; }

        IQueryable<Contact> IRepository.Get()
        {
            return this.Contacts;
        }

        void IRepository.Add(Contact contact)
        {
            this.Contacts.Add(contact);
        }

        void IRepository.SaveChanges()
        {
            this.SaveChanges();
        }
    }
}
