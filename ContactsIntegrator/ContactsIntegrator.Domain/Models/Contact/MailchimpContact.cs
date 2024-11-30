using System.ComponentModel.DataAnnotations;

namespace ContactsIntegrator.Domain.Models.Contact
{
    public record MailchimpContact
    {
        /// <summary>
        /// First name of the contact
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name of the contact
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Email of the contact
        /// </summary>
        [EmailAddress]
        public string Email { get; set; }
    }
}
