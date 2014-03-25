namespace ContactManager.ViewModels
{
    public class MainViewModel : ViewModel
    {
        public MainViewModel()
        {
            this.NewContact = new NewContactViewModel();
            this.ContactList = new ContactListViewModel();
        }

        public NewContactViewModel NewContact { get; private set; }
        public ContactListViewModel ContactList { get; private set; }
    }
}