using AutoMapper;
using ContactsIntegrator.Domain.Interfaces.Infrastructure;
using ContactsIntegrator.Domain.Models.Contact;
using ContactsIntegrator.SDK.ContactsApi.DTOs;
using Newtonsoft.Json;

namespace ContactsIntegrator.SDK.ContactsApi
{
    public class ContactsApiClient : IContactsApiClient
    {
        private readonly IMapper _mapper;
        private readonly IHttpClientFactory _httpClientFactory;
        public ContactsApiClient(IMapper mapper, IHttpClientFactory httpClientFactory)
        {
            _mapper = mapper;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IEnumerable<ExternalApiContact>> GetContactsAsync()
        {
            var result = new List<ExternalApiContact>();
            var httpClient = _httpClientFactory.CreateClient(nameof(ContactsApiClient));
            var request = new HttpRequestMessage(HttpMethod.Get, "api/v1/contacts");
            var response = await httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var responseContacts = JsonConvert.DeserializeObject<List<Contact>>(responseString);
                result = _mapper.Map<List<ExternalApiContact>>(responseContacts);
            }
            else
            {
                Console.WriteLine($"Failed to get contacts from external API - Response code {response.StatusCode} - {response.ReasonPhrase}");
            }

            return result;
        }
    }
}
