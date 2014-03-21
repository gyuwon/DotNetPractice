using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using ContactManager.Models;

namespace ContactManager.Controllers
{
    public class ContactsController : ApiController
    {
        private IRepository _repo;

        public ContactsController(IRepository repo)
        {
            this._repo = repo;
        }

        public IEnumerable<Contact> Get()
        {
            return this._repo.Get().ToList();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            this._repo.Dispose();
        }

        public Contact Post(Contact contact)
        {
            var query = from e in this._repo.Get()
                        where e.Email == contact.Email
                        select e;
            var entity = query.SingleOrDefault();
            if (entity != null)
                throw new HttpResponseException(HttpStatusCode.Conflict);

            this._repo.Add(contact);
            this._repo.SaveChanges();
            return contact;
        }
    }
}
