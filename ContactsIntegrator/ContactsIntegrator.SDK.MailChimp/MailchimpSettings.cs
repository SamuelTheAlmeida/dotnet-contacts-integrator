namespace ContactsIntegrator.SDK.MailChimp
{
    public record MailchimpSettings
    {
        public string? BaseUrl { get; init; }
        public string? AddContactEndpoint { get; init; }
        public string? ApiKey { get; init; }
        public string? ListId { get; init; }
    }
}