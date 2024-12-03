using ContactsIntegrator.Domain.Models.Contact;

namespace ContactsIntegrator.Domain.Interfaces.Services
{
    public interface IContactsIntegrationService
    {
        Task<SyncContactsResult> SynchronizeContactsAsync();
    }
}
