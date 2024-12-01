using ContactsIntegrator.Application.DTOs;
using ContactsIntegrator.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContactsIntegrator.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IContactsIntegrationService _contactsIntegrationService;
        public ContactsController(IContactsIntegrationService contactsIntegrationService)
        {
            _contactsIntegrationService = contactsIntegrationService;
        }

        [HttpGet("contacts/sync")]
        public async Task<ActionResult<SyncContactsResponse>> SyncContacts()
        {
            var response = new SyncContactsResponse
            {
                SyncedContacts = 1,
                Contacts = new List<SyncContactsResponse.SyncedContact>
                {
                    new SyncContactsResponse.SyncedContact
                    {
                        FirstName = "John", LastName = "Doe", Email = "johndoe@mail.com"
                    }
                }
            };
            return Ok(response);
        }
    }
}
