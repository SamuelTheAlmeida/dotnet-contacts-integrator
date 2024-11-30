using System.ComponentModel.DataAnnotations;

namespace ContactsIntegrator.Domain.Models.Contact
{
    public record ExternalApiContact
    {
        /// <summary>
        /// Creation date of the contact
        /// </summary>
        public DateTime CreatedAt { get; set; }

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

        /// <summary>
        /// Avatar picture of the contact (URL)
        /// </summary>
        [Url]
        public string Avatar { get; set; }

        /// <summary>
        /// Unique Id of the contact from the external source
        /// </summary>
        [Key]
        public long Id { get; set; }
    }
}
