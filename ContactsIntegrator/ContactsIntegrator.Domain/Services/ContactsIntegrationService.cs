using ContactsIntegrator.Domain.Interfaces.Infrastructure;
using ContactsIntegrator.Domain.Interfaces.Services;
using ContactsIntegrator.Domain.Models.Contact;

namespace ContactsIntegrator.Domain.Services
{
    public class ContactsIntegrationService(IContactsApiClient contactsApiClient, IMailchimpClient mailchimpClient) : IContactsIntegrationService
    {
        public async Task<SyncContactsResult> SynchronizeContactsAsync()
        {
            var result = new SyncContactsResult();
            var externalApiContacts = await contactsApiClient.GetContactsAsync();
            foreach (var externalApiContact in externalApiContacts)
            {
                var syncResult = await mailchimpClient.AddContactAsync(externalApiContact);
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
