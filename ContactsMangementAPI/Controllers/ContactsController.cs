using ContactsMangementAPI.Models;
using ContactsMangementAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContactsMangementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IContactService _contactService;

        public ContactsController(IContactService contactService)
        {
            _contactService = contactService;
        }

        [HttpGet]
        public ActionResult<List<Contact>> GetContacts()
        {
            try
            {
                return Ok(_contactService.GetContacts());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Contact> GetContactById(int id)
        {
            try
            {
                var contact = _contactService.GetContactById(id);
                if (contact == null)
                    return NotFound("Contact not found.");

                return Ok(contact);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult CreateContact([FromBody] Contact contact)
        {
            try
            {
                if (_contactService.ValidateContact(contact))
                {
                    _contactService.CreateContact(contact);
                    return CreatedAtAction(nameof(GetContactById), new { id = contact.Id }, contact);
                }

                return BadRequest("Validation failed.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public ActionResult UpdateContact(int id, [FromBody] Contact contact)
        {
            try
            {
                if (_contactService.ValidateContact(contact))
                {
                    _contactService.UpdateContact(id, contact);
                    return NoContent();
                }

                return BadRequest("Validation failed.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteContact(int id)
        {
            try
            {
                _contactService.DeleteContact(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
