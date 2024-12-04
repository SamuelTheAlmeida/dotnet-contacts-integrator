namespace ContactsIntegrator.SDK.ContactsApi
{
    public record ContactsApiSettings
    {
        public string? BaseUrl { get; init; }
        public string? GetContactsEndpoint { get; init; }
    }
}
