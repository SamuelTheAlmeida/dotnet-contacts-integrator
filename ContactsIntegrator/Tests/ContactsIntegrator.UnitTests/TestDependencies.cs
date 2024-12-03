using AutoMapper;
using ContactsIntegrator.Application.MappingProfiles;
using Microsoft.Extensions.DependencyInjection;

namespace ContactsIntegrator.UnitTests
{
    public class TestDependencies
    {
        protected readonly IMapper Mapper;
        public TestDependencies()
        {
            var services = new ServiceCollection();
            var provider = services.BuildServiceProvider();

            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new ContactsApiProfile());
                cfg.AddProfile(new MailchimpProfile());
            });
            Mapper = new Mapper(configuration);
        }
    }
}
