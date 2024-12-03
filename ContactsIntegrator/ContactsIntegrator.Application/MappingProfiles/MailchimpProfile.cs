using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                .ForMember(dest => dest.MergeFields.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.MergeFields.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Status, opt => opt.Ignore());
        }
    }
}
