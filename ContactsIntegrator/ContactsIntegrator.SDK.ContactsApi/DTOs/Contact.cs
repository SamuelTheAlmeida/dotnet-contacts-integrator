namespace ContactsIntegrator.SDK.ContactsApi.DTOs
{
    public record Contact
    {
        public long Id { get; init; }
        public string? FirstName { get; init; }
        public string? LastName { get; init; }
        public string? Avatar { get; init; }
        public string Email { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}
