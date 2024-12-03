using ContactsIntegrator.Domain.Interfaces.Infrastructure;
using ContactsIntegrator.Domain.Models.Contact;
using ContactsIntegrator.Domain.Services;
using Moq;
using Xunit;

namespace ContactsIntegrator.UnitTests.ServiceTests
{
    public class ContactsIntegrationServiceTests
    {
        public ContactsIntegrationServiceTests()
        {
            
        }

        [Fact]
        public async Task SynchronizeContacts_Success_ShouldSyncContactsToMailchimp()
        {
            // Arrange
            var contactsApiClientMock = new Mock<IContactsApiClient>();
            contactsApiClientMock.Setup(x => x.GetContactsAsync()).ReturnsAsync(new List<ExternalApiContact>
            {
                new ExternalApiContact
                {
                    Avatar = "https://test.com/avatar.jpg",
                    CreatedAt = DateTime.Now.AddDays(-1),
                    Email = "test1@mail.com",
                    FirstName = "Test",
                    Id = 115661,
                    LastName = "User"
                }
            });

            var mailchimpClientMock = new Mock<IMailchimpClient>();
            mailchimpClientMock.Setup(x => x.AddContactAsync(It.IsAny<MailchimpContact>())).ReturnsAsync(new MailchimpContact
            {
                Email = "test1@mail.com",
                FirstName = "Test",
                LastName = "User"
            });

            var contactsIntegrationService = new ContactsIntegrationService(contactsApiClientMock.Object, mailchimpClientMock.Object);

            // Act
            var result = await contactsIntegrationService.SynchronizeContactsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.NotEmpty(result.Contacts);
            Assert.Equal(1, result.SyncedContacts);
            Assert.Equal("test1@mail.com", result.Contacts[0].Email);
            contactsApiClientMock.Verify(x => x.GetContactsAsync(), Times.Once);
            mailchimpClientMock.Verify(x => x.AddContactAsync(It.IsAny<MailchimpContact>()), Times.Once);
        }
    }
}
