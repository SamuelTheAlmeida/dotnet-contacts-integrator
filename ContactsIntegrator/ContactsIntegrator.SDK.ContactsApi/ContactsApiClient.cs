using AutoMapper;
using ContactsIntegrator.Domain.Interfaces.Infrastructure;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Contact = ContactsIntegrator.Domain.Models.Contact.Contact;

namespace ContactsIntegrator.SDK.ContactsApi
{
    public class ContactsApiClient : IContactsApiClient
    {
        private readonly IMapper _mapper;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ContactsApiSettings _contactsApiSettings;
        private readonly ILogger<ContactsApiClient> _logger;
        public ContactsApiClient(IMapper mapper, IHttpClientFactory httpClientFactory, ContactsApiSettings contactsApiSettings, ILogger<ContactsApiClient> logger)
        {
            _mapper = mapper;
            _httpClientFactory = httpClientFactory;
            _contactsApiSettings = contactsApiSettings;
            _logger = logger;
        }

        public async Task<IEnumerable<Contact>> GetContactsAsync()
        {
            try
            {
                List<Contact> result;
                var httpClient = _httpClientFactory.CreateClient(nameof(ContactsApiClient));
                var request = new HttpRequestMessage(HttpMethod.Get, _contactsApiSettings.GetContactsEndpoint);
                var response = await httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    var responseContacts = JsonConvert.DeserializeObject<List<DTOs.Contact>>(responseString);
                    result = _mapper.Map<List<Contact>>(responseContacts);
                }
                else
                    throw new HttpRequestException($"{response.StatusCode} - {response.ReasonPhrase}");

                return result;
            }
            catch (Exception ex)
            {
                var errorMessage = $"Failed to fetch contacts from external API: {ex.Message}";
                _logger.LogError(ex, errorMessage);
                throw new Exception(errorMessage, ex);
            }
        }
    }
}
