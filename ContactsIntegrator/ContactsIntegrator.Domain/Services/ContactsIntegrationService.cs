using ContactsIntegrator.Domain.Interfaces.Infrastructure;
using ContactsIntegrator.Domain.Interfaces.Services;

namespace ContactsIntegrator.Domain.Services
{
    public class ContactsIntegrationService : IContactsIntegrationService
    {
        private readonly IContactsApiClient _contactsApiClient;
        private readonly IMailchimpClient _mailchimpClient;
        
        public ContactsIntegrationService(IContactsApiClient contactsApiClient, IMailchimpClient mailchimpClient)
        {
            _contactsApiClient = contactsApiClient;
            _mailchimpClient = mailchimpClient;
        }
    }
}
