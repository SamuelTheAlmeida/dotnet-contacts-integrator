using System.Net;
using ContactsIntegrator.Domain.Models.Contact;
using ContactsIntegrator.SDK.ContactsApi;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Xunit;

namespace ContactsIntegrator.UnitTests.SDKTests
{
    public class ContactsApiTests : TestDependencies
    {
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock = new();
        private readonly Mock<ILogger<ContactsApiClient>> _loggerMock = new();

        [Fact]
        public async Task ContactsApi_GetContactsAsync_ReturnsContacts()
        {
            // Arrange
            var contactsResponse = GetListOfContacts();

            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(contactsResponse))
                });

            _httpClientFactoryMock.Setup(_httpClientFactoryMock => _httpClientFactoryMock.CreateClient(It.IsAny<string>()))
                .Returns(new HttpClient(mockMessageHandler.Object) {  
                    BaseAddress = new Uri("http://fake-api-test.com")
                });

            var contactsApiClient = new ContactsApiClient(Mapper, _httpClientFactoryMock.Object, ContactsApiSettings, _loggerMock.Object);

            // Act
            var contacts = await contactsApiClient.GetContactsAsync();

            // Assert
            Assert.NotNull(contacts);
            Assert.NotEmpty(contacts);
            Assert.Equal(contactsResponse.Count, contacts.Count());
            Assert.Equal(contactsResponse.First().Id, contacts.First().Id);
            Assert.Equal(contactsResponse.First().FirstName, contacts.First().FirstName);
            Assert.Equal(contactsResponse.First().LastName, contacts.First().LastName);
            Assert.Equal(contactsResponse.First().Email, contacts.First().Email);
        }

        [Theory]
        [InlineData(true, false, false)]
        [InlineData(false, true, false)]
        [InlineData(false, false, true)]
        public async Task ContactsApi_GetContactsAsync_PartialFieldsResponse_ShouldBeSuccess(bool noFirstName, bool noLastName, bool noAvatar)
        {
            // Arrange
            var contactsResponse = GetListOfContacts(noFirstName, noLastName, noAvatar);

            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(contactsResponse))
                });

            _httpClientFactoryMock.Setup(_httpClientFactoryMock => _httpClientFactoryMock.CreateClient(It.IsAny<string>()))
                .Returns(new HttpClient(mockMessageHandler.Object)
                {
                    BaseAddress = new Uri("http://fake-api-test.com")
                });

            var contactsApiClient = new ContactsApiClient(Mapper, _httpClientFactoryMock.Object, ContactsApiSettings, _loggerMock.Object);

            // Act
            var contacts = await contactsApiClient.GetContactsAsync();

            // Assert
            Assert.NotNull(contacts);
            Assert.NotEmpty(contacts);
            Assert.Equal(contactsResponse.Count, contacts.Count());
            Assert.Equal(contactsResponse.First().Id, contacts.First().Id);
            Assert.Equal(contactsResponse.First().FirstName, contacts.First().FirstName);
            Assert.Equal(contactsResponse.First().LastName, contacts.First().LastName);
            Assert.Equal(contactsResponse.First().Email, contacts.First().Email);
        }

        [Fact]
        public async Task ContactsApi_GetContactsAsync_EmptySuccessResponse_ShouldReturnEmptyList()
        {
            // Arrange
            var contactsResponse = new List<Contact>();

            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(contactsResponse))
                });

            _httpClientFactoryMock.Setup(_httpClientFactoryMock => _httpClientFactoryMock.CreateClient(It.IsAny<string>()))
                .Returns(new HttpClient(mockMessageHandler.Object)
                {
                    BaseAddress = new Uri("http://fake-api-test.com")
                });

            var contactsApiClient = new ContactsApiClient(Mapper, _httpClientFactoryMock.Object, ContactsApiSettings, _loggerMock.Object);

            // Act
            var contacts = await contactsApiClient.GetContactsAsync();

            // Assert
            Assert.NotNull(contacts);
            Assert.Empty(contacts);
        }

        [Theory]
        [InlineData(HttpStatusCode.InternalServerError)]
        [InlineData(HttpStatusCode.BadRequest)]
        [InlineData(HttpStatusCode.BadGateway)]
        [InlineData(HttpStatusCode.GatewayTimeout)]
        [InlineData(HttpStatusCode.Unauthorized)]
        [InlineData(HttpStatusCode.Forbidden)]
        [InlineData(HttpStatusCode.MethodNotAllowed)]
        public async Task ContactsApi_GetContactsAsync_FailureResponse_ShouldThrowException(HttpStatusCode statusCode)
        {
            // Arrange
            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("An error occurred while sending the request", null, statusCode));

            _httpClientFactoryMock.Setup(_httpClientFactoryMock => _httpClientFactoryMock.CreateClient(It.IsAny<string>()))
                .Returns(new HttpClient(mockMessageHandler.Object)
                {
                    BaseAddress = new Uri("http://fake-api-test.com")
                });

            var contactsApiClient = new ContactsApiClient(Mapper, _httpClientFactoryMock.Object, ContactsApiSettings, _loggerMock.Object);

            // Act & Assert
            await Assert.ThrowsAnyAsync<Exception>(async () =>
            {
                await contactsApiClient.GetContactsAsync();
            });
        }

        private List<SDK.ContactsApi.DTOs.Contact> GetListOfContacts(bool noFirstName = false, bool noLastName = false, bool noAvatar = false)
        {
            return new List<SDK.ContactsApi.DTOs.Contact>
            {
                new SDK.ContactsApi.DTOs.Contact
                {
                    Id = 1,
                    FirstName = noFirstName ? default : "John",
                    LastName = noLastName ? default : "Doe",
                    Email = "johndoe@fake.com",
                    Avatar = noAvatar ? default : "https://avatar.com/johndoe.jpg",
                    CreatedAt = DateTime.Now.AddDays(-300)
                },
                new SDK.ContactsApi.DTOs.Contact
                {
                    Id = 2,
                    FirstName = noFirstName ? default : "Samuel",
                    LastName = noLastName ? default : "Almeida",
                    Email = "samuel@fake.com",
                    Avatar = noAvatar ? default : "https://avatar.com/samuel.jpg",
                    CreatedAt = DateTime.Now.AddDays(-100)
                }
            };
        }
    }
}
