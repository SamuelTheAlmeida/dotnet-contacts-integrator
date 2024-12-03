using AutoMapper;
using ContactsIntegrator.Domain.Models.Contact;
using ContactsIntegrator.SDK.MailChimp.DTOs;

namespace ContactsIntegrator.Application.MappingProfiles
{
    public class MailchimpProfile : Profile
    {
        public MailchimpProfile()
        {
            CreateMap<MailchimpContact, AddContactRequest>()
                .ForMember(dest => dest.Status, opt => opt.Ignore());

            CreateMap<MailchimpContact, MergeFields>();
        }
    }
}
