using AutoMapper;
using ContactsIntegrator.Api.Middlewares;
using ContactsIntegrator.Application.MappingProfiles;
using ContactsIntegrator.Domain.Interfaces.Infrastructure;
using ContactsIntegrator.Domain.Interfaces.Services;
using ContactsIntegrator.Domain.Services;
using ContactsIntegrator.SDK.ContactsApi;
using ContactsIntegrator.SDK.MailChimp;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;

namespace ContactsIntegrator.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();

            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Contacts Integrator API",
                    Description = "An API for synchronizing contacts from an external API source to a Mailchimp list",
                    Contact = new OpenApiContact
                    {
                        Name = "Samuel T Almeida",
                        Url = new Uri("https://github.com/SamuelTheAlmeida")
                    }
                });
            });

            // AutoMapper
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ContactsApiProfile());
                mc.AddProfile(new MailchimpProfile());
            });

            IMapper mapper = mapperConfig.CreateMapper();
            builder.Services.AddSingleton(mapper);

            // DI - Services
            builder.Services.AddScoped<IContactsIntegrationService, ContactsIntegrationService>();

            // DI  - Infrastructure
            builder.Services.AddScoped<IContactsApiClient, ContactsApiClient>();
            builder.Services.AddScoped<IMailchimpClient, MailchimpClient>();

            // Setup External API Clients with configuration
            ConfigureMailchimpClient(builder.Services, builder.Configuration);
            ConfigureContactsApiClient(builder.Services, builder.Configuration);

            // Exception Handler middleware
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();

            var option = new RewriteOptions();
            option.AddRedirect("^$", "swagger");
            app.UseRewriter(option);

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.UseExceptionHandler();

            app.Run();
        }

        private static void ConfigureMailchimpClient(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var settings = new MailchimpSettings();
            configuration.GetSection(nameof(MailchimpSettings)).Bind(settings);
            serviceCollection.AddSingleton(settings);

            serviceCollection.AddHttpClient(nameof(MailchimpClient), httpClient =>
            {
                if (string.IsNullOrEmpty(settings.BaseUrl))
                {
                    throw new ArgumentException("Missing config MailchimpSettings:BaseUrl");
                }

                httpClient.BaseAddress = new Uri(settings.BaseUrl);

                httpClient.DefaultRequestHeaders.Add(
                    HeaderNames.Authorization, $"Bearer {settings.ApiKey}");
            });
        }

        private static void ConfigureContactsApiClient(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var settings = new ContactsApiSettings();
            configuration.GetSection(nameof(ContactsApiSettings)).Bind(settings);
            serviceCollection.AddSingleton(settings);

            serviceCollection.AddHttpClient(nameof(ContactsApiClient), httpClient =>
            {
                if (string.IsNullOrEmpty(settings.BaseUrl))
                {
                    throw new ArgumentException("Missing config ContactsApiSettings:BaseUrl");
                }
                httpClient.BaseAddress = new Uri(settings.BaseUrl);
            });
        }
    }
}
