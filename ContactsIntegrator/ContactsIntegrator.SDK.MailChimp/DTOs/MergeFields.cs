using Newtonsoft.Json;

namespace ContactsIntegrator.SDK.MailChimp.DTOs
{
    public class MergeFields
    {
        [JsonProperty("FNAME")]
        public string FirstName { get; init; }

        [JsonProperty("LNAME")]
        public string LastName { get; init; }
       
    }
}
