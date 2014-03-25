using ContactManager.Messages;
using ContactManager.Models;
using ContactManager.Services;
using GalaSoft.MvvmLight.Command;
using Microsoft.Practices.ServiceLocation;

namespace ContactManager.ViewModels
{
    public class NewContactViewModel : ViewModel
    {
        private string _firstName;
        private string _lastName;
        private string _email;

        public NewContactViewModel()
        {
            this.Add = new RelayCommand(this.ExecuteAdd);
        }

        public string FirstName
        {
            get
            {
                return this._firstName;
            }
            set
            {
                this.Set(ref this._firstName, value);
            }
        }

        public string LastName
        {
            get
            {
                return this._lastName;
            }
            set
            {
                this.Set(ref this._lastName, value);
            }
        }

        public string Email
        {
            get
            {
                return this._email;
            }
            set
            {
                this.Set(ref this._email, value);
            }
        }

        public RelayCommand Add { get; private set; }

        private async void ExecuteAdd()
        {
            IContactManagerService service = ServiceLocator.Current.GetInstance<IContactManagerService>();
            await service.AddContactAsync(new Contact
            {
                FirstName = this._firstName,
                LastName = this._lastName,
                Email = this._email
            });

            this.FirstName = string.Empty;
            this.LastName = string.Empty;
            this.Email = string.Empty;

            this.MessengerInstance.Send<UpdateMessage, ContactListViewModel>(UpdateMessage.Instance);
        }
    }
}
