using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using ContactManager.Controllers;
using ContactManager.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace ContactManager.Test
{
    [TestClass]
    public class ContactsControllerTest
    {
        private Contact ironman = new Contact { Id = 1, FirstName = "Tony", LastName = "Stark", Email = "ironman@avengers.com" };
        private Contact hulk = new Contact { Id = 2, FirstName = "Bruce", LastName = "Banner", Email = "hulk@avengers.com" };

        [TestMethod]
        public void Get_Should_Return_Contacts()
        {
            // Setup
            var mock = new Mock<IRepository>();
            var contacts = new List<Contact> { ironman, hulk };
            mock.Setup(r => r.Get()).Returns(contacts.AsQueryable());
            IRepository repository = mock.Object;
            var controller = new ContactsController(repository);

            // Exercise
            var result = controller.Get().ToList();

            // Verify
            mock.Verify(r => r.Get(), Times.Once());
            result.Should().NotBeNull();
            result.Should().HaveCount(contacts.Count);
            result[0].Should().Be(contacts[0]);
            result[1].Should().Be(contacts[1]);
        }

        [TestMethod]
        public void Post_Should_Call_Add_And_SaveChanges()
        {
            // Setup
            var mock = new Mock<IRepository>();
            var contact = ironman;
            mock.Setup(r => r.Add(contact)).Verifiable();
            mock.Setup(r => r.SaveChanges()).Verifiable();
            IRepository repository = mock.Object;
            var controller = new ContactsController(repository);

            // Exercise
            Contact result = controller.Post(contact);

            // Verify
            mock.VerifyAll();
            result.Should().NotBeNull();
            result.Should().Be(contact);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void Post_Should_Throw_Error_With_Duplicated_Email()
        {
            // Setup
            var mock = new Mock<IRepository>();
            var contact = ironman;
            mock.Setup(r => r.Get()).Returns(new Contact[] { contact }.AsQueryable()).Verifiable();
            IRepository repository = mock.Object;
            var controller = new ContactsController(repository);

            try
            {
                // Exercise
                controller.Post(contact);
            }
            finally
            {
                // Verify
                mock.VerifyAll();
            }
        }
    }
}
