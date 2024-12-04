using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System.Net;
using ContactsIntegrator.Domain.Models.Contact;
using ContactsIntegrator.SDK.MailChimp;
using ContactsIntegrator.SDK.MailChimp.DTOs;
using Microsoft.Extensions.Logging;
using Xunit;
using ContactsIntegrator.SDK.ContactsApi;

namespace ContactsIntegrator.UnitTests.SDKTests
{
    public class MailchimpApiTests : TestDependencies
    {
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock = new();
        private readonly Mock<ILogger<MailchimpClient>> _loggerMock = new();

        [Fact]
        public async Task MailchimpApi_AddContactAsync_ShouldReturnTheAddedContact()
        {
            // Arrange
            var mailchimpResponse = new AddContactResponse
            {
                Email = "samuel@fake.com",
                MergeFields = new MergeFields
                {
                    FirstName = "John",
                    LastName = "Does",
                }
            };

            var mockMessageHandler = new Mock<HttpMessageHandler>();
            mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(mailchimpResponse))
                });

            _httpClientFactoryMock.Setup(_httpClientFactoryMock => _httpClientFactoryMock.CreateClient(It.IsAny<string>()))
                .Returns(new HttpClient(mockMessageHandler.Object)
                {
                    BaseAddress = new Uri("http://fake-api-test.com")
                });

            var mailchimpClient = new MailchimpClient(Mapper, _httpClientFactoryMock.Object, MailchimpSettings, _loggerMock.Object);

            var requestContact = GetRequestContact();

            // Act
            var contactResponse = await mailchimpClient.AddContactAsync(requestContact);

            // Assert
            Assert.NotNull(contactResponse);
            Assert.Equal(contactResponse.FirstName, requestContact.FirstName);
            Assert.Equal(contactResponse.LastName, requestContact.LastName);
            Assert.Equal(contactResponse.Email, requestContact.Email);
        }

        [Theory]
        [InlineData(HttpStatusCode.InternalServerError)]
        [InlineData(HttpStatusCode.BadRequest)]
        [InlineData(HttpStatusCode.BadGateway)]
        [InlineData(HttpStatusCode.GatewayTimeout)]
        [InlineData(HttpStatusCode.Unauthorized)]
        [InlineData(HttpStatusCode.Forbidden)]
        [InlineData(HttpStatusCode.MethodNotAllowed)]
        public async Task MailchimpApi_AddContactAsync_FailureResponse_ShouldThrowException(HttpStatusCode statusCode)
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

            var mailchimpClient = new MailchimpClient(Mapper, _httpClientFactoryMock.Object, MailchimpSettings, _loggerMock.Object);

            // Act & Assert
            await Assert.ThrowsAnyAsync<Exception>(async () =>
            {
                await mailchimpClient.AddContactAsync(GetRequestContact());
            });
        }

        private Contact GetRequestContact()
        {
            return new Contact
            {
                Email = "samuel@fake.com",
                FirstName = "John",
                LastName = "Does"
            };
        }
    }
}
