using System.Net;
using ContactsIntegrator.SDK.ContactsApi;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Xunit;

namespace ContactsIntegrator.UnitTests.SDKTests
{
    public class ContactsApiTests : TestDependencies
    {
        private Mock<IHttpClientFactory> _httpClientFactoryMock;
        public ContactsApiTests()
        {
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
        }

        [Fact]
        public async Task ContactsApi_GetContactsAsync_ReturnsContacts()
        {
            // Arrange
            var contactsResponse = new List<SDK.ContactsApi.DTOs.Contact>
            {
                new SDK.ContactsApi.DTOs.Contact
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "johndoe@fake.com",
                    Avatar = "https://avatar.com/johndoe.jpg",
                    CreatedAt = DateTime.Now.AddDays(-300)
                },
                new SDK.ContactsApi.DTOs.Contact
                {
                    Id = 2,
                    FirstName = "Sam",
                    LastName = "Almeida",
                    Email = "samuel@fake.com",
                    Avatar = "https://avatar.com/samuel.jpg",
                    CreatedAt = DateTime.Now.AddDays(-100)
                }
            };

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

            var contactsApiClient = new ContactsApiClient(Mapper, _httpClientFactoryMock.Object);

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
    }
}
