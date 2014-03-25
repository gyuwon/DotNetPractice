using System.Linq;
using ContactManager.Messages;
using ContactManager.Models;
using ContactManager.Services;
using ContactManager.ViewModels;
using FluentAssertions;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Practices.ServiceLocation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ContactManager.Test.ViewModels
{
    [TestClass]
    public class ContactListViewModelTest
    {
        private Contact ironman = new Contact { Id = 1, FirstName = "Tony", LastName = "Stark", Email = "ironman@avengers.com" };
        private Contact hulk = new Contact { Id = 2, FirstName = "Bruce", LastName = "Banner", Email = "hulk@avengers.com" };

        [TestInitialize]
        public void Initialize()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
        }

        [TestMethod]
        public void ContactListViewModel_Should_Update_Contacts_Property_With_Service_When_Receive_UpdateMessage()
        {
            // Setup
            var mock = new Mock<IContactManagerService>();
            mock.Setup(s => s.GetContactsAsync()).ReturnsAsync(new Contact[] { ironman, hulk });
            var service = mock.Object;
            SimpleIoc.Default.Register<IContactManagerService>(() => service);
            ContactListViewModel vm = new ContactListViewModel();

            // Exercise
            Messenger.Default.Send(UpdateMessage.Instance);

            // Verify
            mock.VerifyAll();
            vm.Contacts.Should().NotBeNull();
            vm.Contacts.Count().Should().Be(2);
            vm.Contacts.First().Should().Be(ironman);
            vm.Contacts.Skip(1).First().Should().Be(hulk);
        }
    }
}
