using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        /*[Fact]
        public async Task SyncContacts_Success()
        {
            // Arrange
            var contactsApiClientMock = new Mock<IContactsApiClient>();
            contactsApiClientMock.Setup(x => x.GetContacts()).ReturnsAsync(new List<ExternalApiContact>());

            var mailchimpClientMock = new Mock<IMailchimpClient>();
            mailchimpClientMock.Setup(x => x.AddListAsync()).ReturnsAsync(new MailchimpListResponse());
            mailchimpClientMock.Setup(x => x.AddMemberAsync(It.IsAny<string>(), It.IsAny<MailchimpMemberRequest>())).ReturnsAsync(new MailchimpMemberResponse());

            var contactsIntegrationService = new ContactsIntegrationService(contactsApiClientMock.Object, mailchimpClientMock.Object);

            // Act
            var result = await contactsIntegrationService.SyncContacts();

            // Assert
            Assert.NotNull(result);
        }*/
    }
}
