using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using ContactsIntegrator.Domain.Interfaces.Infrastructure;
using ContactsIntegrator.Domain.Models.Contact;
using ContactsIntegrator.SDK.MailChimp.DTOs;
using Newtonsoft.Json;

namespace ContactsIntegrator.SDK.MailChimp
{
    public class MailchimpClient : IMailchimpClient
    {
        private readonly IMapper _mapper;
        private readonly IHttpClientFactory _httpClientFactory;
        public MailchimpClient(IMapper mapper, IHttpClientFactory httpClientFactory)
        {
            _mapper = mapper;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<MailchimpContact?> AddContactAsync(MailchimpContact contact)
        {
            var httpClient = _httpClientFactory.CreateClient(nameof(MailchimpClient));
            var listId = "bd00329228";
            var contactEmailMd5 = GenerateMd5Hash(contact.Email);
            var endpoint = string.Format("lists/{0}/members,{1}", listId, contactEmailMd5);
            var request = new HttpRequestMessage(HttpMethod.Put, endpoint);

            var addContactRequest = _mapper.Map<AddContactRequest>(contact);
            var content = new StringContent(JsonConvert.SerializeObject(addContactRequest), Encoding.UTF8, "application/json");
            request.Content = content;
            var response = await httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Failed to add contact to Mailchimp - Response code {response.StatusCode} - {response.ReasonPhrase}");
            }
            else
            {
                var responseContentString = await response.Content.ReadAsStringAsync();
                var addContactResponse = JsonConvert.DeserializeObject<AddContactResponse>(responseContentString);
                return _mapper.Map<MailchimpContact>(addContactResponse);
            }

            return null;
        }

        private static string GenerateMd5Hash(string email)
        {
            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.UTF8.GetBytes(email);
                var hashBytes = md5.ComputeHash(inputBytes);
                var sb = new StringBuilder();
                for (var i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}
