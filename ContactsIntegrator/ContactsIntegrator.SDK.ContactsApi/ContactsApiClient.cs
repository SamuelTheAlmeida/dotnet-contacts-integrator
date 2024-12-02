using AutoMapper;
using ContactsIntegrator.Domain.Interfaces.Infrastructure;
using ContactsIntegrator.Domain.Models.Contact;
using ContactsIntegrator.SDK.ContactsApi.DTOs;
using Newtonsoft.Json;
using RestSharp;

namespace ContactsIntegrator.SDK.ContactsApi
{
    public class ContactsApiClient : IContactsApiClient
    {
        private readonly IMapper _mapper;
        private readonly IRestClient _restClient;
        public ContactsApiClient(IMapper mapper)
        {
            _mapper = mapper;

            var options = new RestClientOptions("https://challenge.trio.dev/api/v1");
            _restClient = new RestClient(options);
        }

        public async Task<IEnumerable<ExternalApiContact>> GetContactsAsync()
        {
            var result = new List<ExternalApiContact>();

            var request = new RestRequest("contacts");
            var response = await _restClient.ExecuteAsync(request);
            var isSuccess = response is { IsSuccessful: true, Content: not null };
            if (isSuccess)
            {
                var responseContacts = JsonConvert.DeserializeObject<List<Contact>>(response.Content);
                result = _mapper.Map<List<ExternalApiContact>>(responseContacts);

            }
            else
            {
                Console.WriteLine($"Failed to get contacts from external API - Response code {response.StatusCode} - {response.StatusDescription}");
            }

            return result;
        }
    }
}
