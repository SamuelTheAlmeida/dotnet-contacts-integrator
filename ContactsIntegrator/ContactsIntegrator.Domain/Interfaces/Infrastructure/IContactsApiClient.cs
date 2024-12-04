using ContactsIntegrator.Domain.Models.Contact;

namespace ContactsIntegrator.Domain.Interfaces.Infrastructure
{
    public interface IContactsApiClient
    {
        Task<IEnumerable<Contact>> GetContactsAsync();
    }
}
