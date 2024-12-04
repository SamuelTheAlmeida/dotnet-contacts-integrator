using AutoMapper;
using ContactsIntegrator.Application.MappingProfiles;
using ContactsIntegrator.SDK.ContactsApi;
using ContactsIntegrator.SDK.MailChimp;
using Microsoft.Extensions.DependencyInjection;

namespace ContactsIntegrator.UnitTests
{
    public class TestDependencies
    {
        protected readonly IMapper Mapper;
        protected readonly ContactsApiSettings ContactsApiSettings;
        protected readonly MailchimpSettings MailchimpSettings;

        public TestDependencies()
        {
            var services = new ServiceCollection();
            services.BuildServiceProvider();

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ContactsApiProfile());
                cfg.AddProfile(new MailchimpProfile());
            });
            Mapper = new Mapper(configuration);

            ContactsApiSettings = new ContactsApiSettings
                { BaseUrl = "http://test.com", GetContactsEndpoint = "test" };
            MailchimpSettings = new MailchimpSettings
            {
                BaseUrl = "http://test.com",
                AddContactEndpoint = "test",
                ApiKey = "test123",
                ListId = "testList"
            };
        }
    }
}
