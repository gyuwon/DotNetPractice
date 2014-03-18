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
            var entity = this._repo.Get().SingleOrDefault(e => e.Email == contact.Email);
            if (entity != null)
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            this._repo.Add(contact);
            this._repo.SaveChanges();
            return contact;
        }
    }
}
