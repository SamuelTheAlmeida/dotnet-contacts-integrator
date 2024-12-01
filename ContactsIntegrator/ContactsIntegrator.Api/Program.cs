using ContactsIntegrator.Domain.Interfaces.Infrastructure;
using ContactsIntegrator.Domain.Interfaces.Services;
using ContactsIntegrator.Domain.Services;
using ContactsIntegrator.SDK.ContactsApi;
using ContactsIntegrator.SDK.MailChimp;

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
            builder.Services.AddSwaggerGen();

            // DI - Services
            builder.Services.AddScoped<IContactsIntegrationService, ContactsIntegrationService>();

            // DI  - Infrastructure
            builder.Services.AddScoped<IContactsApiClient, ContactsApiClient>();
            builder.Services.AddScoped<IMailchimpClient, MailchimpClient>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
