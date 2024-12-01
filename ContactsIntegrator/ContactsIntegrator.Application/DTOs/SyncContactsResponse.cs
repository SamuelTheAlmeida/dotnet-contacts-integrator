namespace ContactsIntegrator.Application.DTOs
{
    public record SyncContactsResponse
    {
        /// <summary>
        /// Amount of contacts sent over to Mailchimp
        /// </summary>
        public long SyncedContacts { get; init; }

        /// <summary>
        /// List of contacts that were synced
        /// </summary>

        public IEnumerable<SyncedContact> Contacts { get; init; }

        public record SyncedContact
        {
            /// <summary>
            /// First name of the contact
            /// </summary>
            public string FirstName { get; init; }

            /// <summary>
            /// Last name of the contact
            /// </summary>
            public string LastName { get; init; }

            /// <summary>
            /// Email address of the contact
            /// </summary>
            public string Email { get; init; }
        }
    }
}
