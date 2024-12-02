using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ContactsIntegrator.SDK.ContactsApi;
using Moq;
using Xunit;

namespace ContactsIntegrator.UnitTests.SDKTests
{
    public class ContactsApiTests
    {
        private Mock<IMapper> _mapperMock;
        public ContactsApiTests()
        {
            _mapperMock = new Mock<IMapper>();
        }

        [Fact]
        public async Task ContactsApi_GetContactsAsync_ReturnsContacts()
        {
            // Arrange
            var contactsApiClient = new ContactsApiClient(_mapperMock.Object);

            // Act
            var contacts = await contactsApiClient.GetContactsAsync();

            // Assert
            Assert.NotNull(contacts);
            Assert.NotEmpty(contacts);
        }
    }
}
