using ContactsMangementAPI.Models;
using ContactsMangementAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContactsMangementAPI.Controllers
{
    /// <summary>
    /// Controller to manage contact operations, such as retrieval, creation, updating, and deletion.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IContactService _contactService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContactsController"/> class.
        /// </summary>
        /// <param name="contactService">The contact service instance for managing contact data.</param>
        public ContactsController(IContactService contactService)
        {
            _contactService = contactService;
        }

        /// <summary>
        /// Retrieves the list of all contacts.
        /// </summary>
        /// <returns>A list of contacts.</returns>
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

        /// <summary>
        /// Retrieves a contact by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the contact.</param>
        /// <returns>The contact with the specified ID.</returns>
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

        /// <summary>
        /// Creates a new contact.
        /// </summary>
        /// <param name="contact">The contact object to be created.</param>
        /// <returns>Action result indicating success or failure of the contact creation.</returns>
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

        /// <summary>
        /// Updates an existing contact by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the contact to be updated.</param>
        /// <param name="contact">The updated contact object.</param>
        /// <returns>Action result indicating success or failure of the update operation.</returns>
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

        /// <summary>
        /// Deletes a contact by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the contact to be deleted.</param>
        /// <returns>Action result indicating success or failure of the delete operation.</returns>
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
