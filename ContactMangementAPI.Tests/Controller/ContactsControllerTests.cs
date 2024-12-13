using ContactsMangementAPI.Controllers;
using ContactsMangementAPI.Models;
using ContactsMangementAPI.Services;
using Moq;
using Microsoft.AspNetCore.Mvc;

namespace ContactsMangementAPI.Tests.Controller
{
    public class ContactsControllerTests
    {
        private readonly Mock<IContactService> _mockContactService;
        private readonly ContactsController _controller;

        public ContactsControllerTests()
        {
            _mockContactService = new Mock<IContactService>();
            _controller = new ContactsController(_mockContactService.Object);
        }

        // Test for GetContacts
        [Fact]
        public void GetContacts_ReturnsOkResult_WithListOfContacts()
        {
            // Arrange
            var mockContacts = new List<Contact>
            {
                new Contact { Id = 1, FirstName = "Nishant", LastName = "M", Email = "nishant_m@gmail.com" },
                new Contact { Id = 2, FirstName = "Jane", LastName = "Doe", Email = "jane@example.com" }
            };

            _mockContactService.Setup(service => service.GetContacts()).Returns(mockContacts);

            // Act
            var result = _controller.GetContacts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var contacts = Assert.IsType<List<Contact>>(okResult.Value);
            Assert.Equal(2, contacts.Count);
        }

        // Test for GetContactById
        [Fact]
        public void GetContactById_ReturnsNotFoundResult_WhenContactDoesNotExist()
        {
            // Arrange
            _mockContactService.Setup(service => service.GetContactById(It.IsAny<int>())).Returns((Contact)null);

            // Act
            var result = _controller.GetContactById(1);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal("Contact not found.", notFoundResult.Value);
        }

        [Fact]
        public void GetContactById_ReturnsOkResult_WithContact_WhenContactExists()
        {
            // Arrange
            var mockContact = new Contact { Id = 1, FirstName = "Nishant", LastName = "M", Email = "nishant_m@gmail.com" };
            _mockContactService.Setup(service => service.GetContactById(1)).Returns(mockContact);

            // Act
            var result = _controller.GetContactById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var contact = Assert.IsType<Contact>(okResult.Value);
            Assert.Equal(1, contact.Id);
            Assert.Equal("John", contact.FirstName);
            Assert.Equal("Doe", contact.LastName);
        }

        // Test for CreateContact
        [Fact]
        public void CreateContact_ReturnsCreatedAtActionResult_WhenContactIsValid()
        {
            // Arrange
            var newContact = new Contact { Id = 3, FirstName = "New", LastName = "Contact", Email = "new@example.com" };
            _mockContactService.Setup(service => service.ValidateContact(It.IsAny<Contact>())).Returns(true);
            _mockContactService.Setup(service => service.CreateContact(It.IsAny<Contact>()));

            // Act
            var result = _controller.CreateContact(newContact);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal("GetContactById", createdAtActionResult.ActionName);
            Assert.Equal(newContact.Id, createdAtActionResult.RouteValues["id"]);
        }

        [Fact]
        public void CreateContact_ReturnsBadRequest_WhenValidationFails()
        {
            // Arrange
            var invalidContact = new Contact { Id = 3, FirstName = "", LastName = "Invalid", Email = "invalid@example.com" };
            _mockContactService.Setup(service => service.ValidateContact(It.IsAny<Contact>())).Returns(false);

            // Act
            var result = _controller.CreateContact(invalidContact);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Validation failed.", badRequestResult.Value);
        }

        // Test for UpdateContact
        [Fact]
        public void UpdateContact_ReturnsNoContentResult_WhenUpdateIsSuccessful()
        {
            // Arrange
            var contactToUpdate = new Contact { Id = 1, FirstName = "Updated", LastName = "Name", Email = "updated@example.com" };
            _mockContactService.Setup(service => service.ValidateContact(It.IsAny<Contact>())).Returns(true);

            // Act
            var result = _controller.UpdateContact(1, contactToUpdate);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void UpdateContact_ReturnsBadRequest_WhenValidationFails()
        {
            // Arrange
            var contactToUpdate = new Contact { Id = 1, FirstName = "", LastName = "Invalid", Email = "invalid@example.com" };
            _mockContactService.Setup(service => service.ValidateContact(It.IsAny<Contact>())).Returns(false);

            // Act
            var result = _controller.UpdateContact(1, contactToUpdate);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Validation failed.", badRequestResult.Value);
        }

        // Test for DeleteContact
        [Fact]
        public void DeleteContact_ReturnsNoContentResult_WhenDeleteIsSuccessful()
        {
            // Arrange
            _mockContactService.Setup(service => service.DeleteContact(It.IsAny<int>()));

            // Act
            var result = _controller.DeleteContact(1);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void DeleteContact_ReturnsBadRequest_WhenAnErrorOccurs()
        {
            // Arrange
            _mockContactService.Setup(service => service.DeleteContact(It.IsAny<int>())).Throws(new Exception("Error"));

            // Act
            var result = _controller.DeleteContact(1);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Error", badRequestResult.Value);
        }
    }
}
