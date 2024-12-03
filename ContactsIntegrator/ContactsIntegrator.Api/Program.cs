using AutoMapper;
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

            // Http Clients
            builder.Services.AddHttpClient(nameof(ContactsApiClient), httpClient =>
            {
                httpClient.BaseAddress = new Uri("https://challenge.trio.dev"); // TODO fetch from config
            });

            builder.Services.AddHttpClient(nameof(MailchimpClient), httpClient =>
            {
                httpClient.BaseAddress = new Uri("https://us15.api.mailchimp.com"); // TODO fetch from config

                httpClient.DefaultRequestHeaders.Add(
                    HeaderNames.Authorization, "Bearer 25d078d164f976c2000a77d94959db61-us15"); //TODO fetch from config
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

            app.Run();
        }
    }
}
