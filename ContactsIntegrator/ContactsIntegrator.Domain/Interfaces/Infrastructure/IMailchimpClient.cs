using ContactsIntegrator.Domain.Models.Contact;

namespace ContactsIntegrator.Domain.Interfaces.Infrastructure
{
    public interface IMailchimpClient
    {
        Task<Contact?> AddContactAsync(Contact contact);
    }
}
