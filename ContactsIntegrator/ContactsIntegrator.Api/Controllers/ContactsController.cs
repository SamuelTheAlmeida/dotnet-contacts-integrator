using AutoMapper;
using ContactsIntegrator.Application.DTOs;
using ContactsIntegrator.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContactsIntegrator.Api.Controllers
{
    [Route("contacts")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IContactsIntegrationService _contactsIntegrationService;
        private readonly IMapper _mapper;
        public ContactsController(IContactsIntegrationService contactsIntegrationService, IMapper mapper)
        {
            _contactsIntegrationService = contactsIntegrationService;
            _mapper = mapper;
        }

        [HttpGet("sync")]
        public async Task<ActionResult<SyncContactsResponse>> SyncContacts()
        {
            var result = _mapper.Map<SyncContactsResponse>(await _contactsIntegrationService.SynchronizeContactsAsync());
            return Ok(result);
        }
    }
}
