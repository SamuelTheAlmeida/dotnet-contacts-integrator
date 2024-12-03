namespace ContactsIntegrator.Domain.Models.Contact
{
    public class SyncContactsResult
    {
        /// <summary>
        /// Amount of contacts sent over to Mailchimp
        /// </summary>
        public long SyncedContacts { get; set; }

        /// <summary>
        /// List of contacts that were synced
        /// </summary>
        public IList<MailchimpContact> Contacts { get; set; }
    }
}
