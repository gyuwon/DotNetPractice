using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ContactManager.Models;
using RestSharp;

namespace ContactManager.Services
{
    public class ContactManagerService : IContactManagerService
    {
        private string _serviceUrl;

        public ContactManagerService(string serviceUrl)
        {
            this._serviceUrl = serviceUrl;
        }

        public async Task<IEnumerable<Contact>> GetContactsAsync()
        {
            var client = new RestClient(this._serviceUrl);
            var req = new RestRequest("Contacts", Method.GET);
            var res = await client.ExecuteTaskAsync<List<Contact>>(req);
            if (res.StatusCode != HttpStatusCode.OK)
                throw new InvalidOperationException(res.StatusDescription);
            return res.Data;
        }

        public async Task<Contact> AddContactAsync(Contact contact)
        {
            var client = new RestClient(this._serviceUrl);
            var req = new RestRequest("Contacts", Method.POST) { RequestFormat = DataFormat.Json }.AddBody(contact);
            var res = await client.ExecuteTaskAsync<Contact>(req);
            if (res.StatusCode != HttpStatusCode.OK)
                throw new InvalidOperationException(res.StatusDescription);
            return res.Data;
        }
    }
}
