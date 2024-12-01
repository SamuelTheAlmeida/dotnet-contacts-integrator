using ContactsIntegrator.Domain.Interfaces.Infrastructure;

namespace ContactsIntegrator.SDK.MailChimp
{
    public class MailchimpClient : IMailchimpClient
    {
        public MailchimpClient() { }

        public async Task AddListAsync()
        {
            var restClient = new RestClient("https://api.mailchimp.com/3.0/lists");
        }
    }
}
