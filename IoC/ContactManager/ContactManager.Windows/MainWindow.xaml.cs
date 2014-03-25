using ContactManager.Messages;
using ContactManager.Services;
using ContactManager.ViewModels;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using MahApps.Metro.Controls;

namespace ContactManager.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            string serviceUrl = "http://localhost:3000/api";
            SimpleIoc.Default.Register<IContactManagerService>(() => new ContactManagerService(serviceUrl));

            Messenger.Default.Send<UpdateMessage, ContactListViewModel>(UpdateMessage.Instance);
        }
    }
}
