using ContactsIntegrator.Api.Controllers;
using ContactsIntegrator.Application.DTOs;
using ContactsIntegrator.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace ContactsIntegrator.UnitTests.ControllerTests
{
    public class ContactsControllerTests : TestDependencies
    {
        public ContactsControllerTests()
        {
            
        }

        [Fact]
        public async Task SyncContacts_Success()
        {
            // Arrange
            var contactsIntegrationServiceMock = new Mock<IContactsIntegrationService>();
            var controller = new ContactsController(contactsIntegrationServiceMock.Object, Mapper);

            // Act
            var response = await controller.SyncContacts();

            // Assert
            Assert.NotNull(response.Result);
            var okResult = Assert.IsType<OkObjectResult>(response.Result);
            var returnedItem = Assert.IsType<SyncContactsResponse>(okResult.Value);
            Assert.NotNull(okResult);
            Assert.NotEmpty(returnedItem.Contacts);
        }

    }
}
