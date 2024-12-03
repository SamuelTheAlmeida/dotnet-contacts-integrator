using AutoMapper;
using ContactsIntegrator.Domain.Models.Contact;

namespace ContactsIntegrator.Application.MappingProfiles
{
    public class ContactsApiProfile : Profile
    {
        public ContactsApiProfile()
        {
            CreateMap<SDK.ContactsApi.DTOs.Contact, ExternalApiContact>();
        }
    }
}
