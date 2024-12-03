using ContactsIntegrator.Domain.Interfaces.Infrastructure;
using ContactsIntegrator.Domain.Interfaces.Services;
using ContactsIntegrator.Domain.Models.Contact;

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

        public async Task<SyncContactsResult> SynchronizeContactsAsync()
        {
            var result = new SyncContactsResult();
            var externalApiContacts = await _contactsApiClient.GetContactsAsync();
            foreach (var externalApiContact in externalApiContacts)
            {
                var mailchimpContact = new MailchimpContact
                {
                    Email = externalApiContact.Email,
                    FirstName = externalApiContact.FirstName,
                    LastName = externalApiContact.LastName
                };
                var syncResult = await _mailchimpClient.AddContactAsync(mailchimpContact);
                if (syncResult != null)
                {
                    result.SyncedContacts++;
                    result.Contacts.Add(syncResult);
                }
            }

            return result;
        }
    }
}
