using ContactsIntegrator.Domain.Interfaces;

namespace ContactsIntegrator.Domain.Services
{
    public class ContactsIntegrationService
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
