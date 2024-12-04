using AutoMapper;
using ContactsIntegrator.Application.DTOs;
using ContactsIntegrator.Domain.Models.Contact;
using ContactsIntegrator.SDK.MailChimp.DTOs;

namespace ContactsIntegrator.Application.MappingProfiles
{
    public class MailchimpProfile : Profile
    {
        public MailchimpProfile()
        {
            CreateMap<Contact, AddContactRequest>()
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.MergeFields, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.EmailAddress, opt => opt.MapFrom(src => src.Email));

            CreateMap<Contact, MergeFields>()
                .ReverseMap();

            CreateMap<AddContactResponse, Contact>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.MergeFields.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.MergeFields.LastName));

            CreateMap<Contact, SyncContactsResponse.SyncedContact>();
        }
    }
}
