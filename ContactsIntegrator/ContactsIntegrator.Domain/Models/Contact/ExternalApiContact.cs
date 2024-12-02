using System.ComponentModel.DataAnnotations;

namespace ContactsIntegrator.Domain.Models.Contact
{
    public record ExternalApiContact
    {
        /// <summary>
        /// Creation date of the contact
        /// </summary>
        public DateTime CreatedAt { get; init; }

        /// <summary>
        /// First name of the contact
        /// </summary>
        public string FirstName { get; init; }

        /// <summary>
        /// Last name of the contact
        /// </summary>
        public string LastName { get; init; }

        /// <summary>
        /// Email of the contact
        /// </summary>
        [EmailAddress]
        public string Email { get; init; }

        /// <summary>
        /// Avatar picture of the contact (URL)
        /// </summary>
        [Url]
        public string Avatar { get; init; }

        /// <summary>
        /// Unique Id of the contact from the external source
        /// </summary>
        [Key]
        public long Id { get; init; }
    }
}
