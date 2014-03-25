using System.Collections.Generic;
using ContactManager.Messages;
using ContactManager.Models;
using ContactManager.Services;
using Microsoft.Practices.ServiceLocation;

namespace ContactManager.ViewModels
{
    public class ContactListViewModel : ViewModel
    {
        private IEnumerable<Contact> _contacts;

        public ContactListViewModel()
        {
            this.MessengerInstance.Register<UpdateMessage>(this, this.OnUpdate);
        }

        private async void OnUpdate(UpdateMessage message)
        {
            IContactManagerService service = ServiceLocator.Current.GetInstance<IContactManagerService>();
            this.Contacts = await service.GetContactsAsync();
        }

        public IEnumerable<Contact> Contacts
        {
            get
            {
                return this._contacts;
            }
            private set
            {
                this.Set(ref this._contacts, value);
            }
        }
    }
}
