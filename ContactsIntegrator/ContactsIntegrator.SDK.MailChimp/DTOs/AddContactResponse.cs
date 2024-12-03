using Newtonsoft.Json;

namespace ContactsIntegrator.SDK.MailChimp.DTOs
{
    public record AddContactResponse
    {
        [JsonProperty("email_address")]
        public string Email { get; init; }

        [JsonProperty("merge_fields")]
        public MergeFields MergeFields { get; init; }
    }
}
