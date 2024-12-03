using Newtonsoft.Json;

namespace ContactsIntegrator.SDK.MailChimp.DTOs
{
    public record MergeFields
    {
        [JsonProperty("FNAME")]
        public string FirstName { get; init; }

        [JsonProperty("LNAME")]
        public string LastName { get; init; }
       
    }
}
