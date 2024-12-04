using System.Net.Mime;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using ContactsIntegrator.Domain.Interfaces.Infrastructure;
using ContactsIntegrator.Domain.Models.Contact;
using ContactsIntegrator.SDK.MailChimp.DTOs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ContactsIntegrator.SDK.MailChimp
{
    public class MailchimpClient : IMailchimpClient
    {
        private readonly IMapper _mapper;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly MailchimpSettings _mailchimpSettings;
        private readonly ILogger<MailchimpClient> _logger;
        public MailchimpClient(IMapper mapper, IHttpClientFactory httpClientFactory, MailchimpSettings mailchimpSettings, ILogger<MailchimpClient> logger)
        {
            _mapper = mapper;
            _httpClientFactory = httpClientFactory;
            _mailchimpSettings = mailchimpSettings;
            _logger = logger;
        }

        public async Task<Contact?> AddContactAsync(Contact contact)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient(nameof(MailchimpClient));
                var contactEmailMd5 = GenerateMd5Hash(contact.Email.ToLower());
                var endpoint = string.Format(_mailchimpSettings.AddContactEndpoint, _mailchimpSettings.ListId, contactEmailMd5);
                var request = new HttpRequestMessage(HttpMethod.Put, endpoint);

                var addContactRequest = _mapper.Map<AddContactRequest>(contact);
                var content = new StringContent(JsonConvert.SerializeObject(addContactRequest), Encoding.UTF8, MediaTypeNames.Application.Json);
                request.Content = content;
                var response = await httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Response code {response.StatusCode} - {response.ReasonPhrase}");
                }

                var responseContentString = await response.Content.ReadAsStringAsync();
                var addContactResponse = JsonConvert.DeserializeObject<AddContactResponse>(responseContentString);
                return _mapper.Map<Contact>(addContactResponse);
            }
            catch (Exception ex)
            {
                var errorMessage = $"Failed to send contacts to Mailchimp: {ex.Message}";
                _logger.LogError(ex, errorMessage);
                throw new Exception(errorMessage, ex);
            }

        }

        private static string GenerateMd5Hash(string email)
        {
            using var md5 = MD5.Create();
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
