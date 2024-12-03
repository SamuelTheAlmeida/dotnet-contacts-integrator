using Newtonsoft.Json;

namespace ContactsIntegrator.SDK.MailChimp.DTOs
{
    public record AddContactRequest
    {
        [JsonProperty("email_address")]
        public string EmailAddress { get; init; }

        [JsonProperty("status")]
        public string Status { get; init; } = MailchimpConstants.StatusSubscribed;

        [JsonProperty("merge_fields")]
        public MergeFields MergeFields { get; init; }
    }
}
