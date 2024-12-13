using Moq;
using Newtonsoft.Json;
using ContactsMangementAPI.Models;
using ContactsMangementAPI.Services;
using ContactsMangementAPI.Handlers;

namespace ContactsMangementAPI.Tests.Controller
{
    public class ContactServiceTests
    {
        private Mock<IFileHandler> _mockFileHandler;
        private IContactService _contactService;

        public ContactServiceTests()
        {
            _mockFileHandler = new Mock<IFileHandler>();
        }

        [Fact]
        public void CreateContact_AddsNewContact_WhenValid()
        {
            var initialContacts = new List<Contact>
        {
            new Contact { Id = 1, FirstName = "Nishant", LastName = "M", Email = "nishant@example.com" }
        };

            _mockFileHandler.Setup(f => f.Exists(It.IsAny<string>())).Returns(true);
            _mockFileHandler.Setup(f => f.ReadAllText(It.IsAny<string>())).Returns(JsonConvert.SerializeObject(initialContacts));
            _mockFileHandler.Setup(f => f.WriteAllText(It.IsAny<string>(), It.IsAny<string>())).Verifiable();

            _contactService = new ContactService(_mockFileHandler.Object);

            var newContact = new Contact { FirstName = "Jane", LastName = "Smith", Email = "jane@example.com" };

            _contactService.CreateContact(newContact);

            _mockFileHandler.Verify(f => f.WriteAllText(It.IsAny<string>(), It.IsAny<string>()), Times.Once);

            var contacts = _contactService.GetContacts();
            Assert.Equal(2, contacts.Count);
        }

        [Fact]
        public void CreateContact_ThrowsException_WhenEmailNotUnique()
        {
            var existingContacts = new List<Contact>
        {
            new Contact { Id = 1, FirstName = "Nishant", LastName = "M", Email = "nishant@example.com" }
        };

            _mockFileHandler.Setup(f => f.Exists(It.IsAny<string>())).Returns(true);
            _mockFileHandler.Setup(f => f.ReadAllText(It.IsAny<string>())).Returns(JsonConvert.SerializeObject(existingContacts));
            _mockFileHandler.Setup(f => f.WriteAllText(It.IsAny<string>(), It.IsAny<string>())).Verifiable();

            _contactService = new ContactService(_mockFileHandler.Object);

            var newContact = new Contact { FirstName = "Jane", LastName = "Smith", Email = "nishant@example.com" };

            var exception = Assert.Throws<InvalidOperationException>(() => _contactService.CreateContact(newContact));
            Assert.Equal("Email must be unique.", exception.Message);
        }

        [Fact]
        public void CreateContact_ThrowsException_WhenInvalidEmail()
        {
            var initialContacts = new List<Contact>
        {
            new Contact { Id = 1, FirstName = "Nishant", LastName = "M", Email = "nishant@example.com" }
        };

            _mockFileHandler.Setup(f => f.Exists(It.IsAny<string>())).Returns(true);
            _mockFileHandler.Setup(f => f.ReadAllText(It.IsAny<string>())).Returns(JsonConvert.SerializeObject(initialContacts));
            _mockFileHandler.Setup(f => f.WriteAllText(It.IsAny<string>(), It.IsAny<string>())).Verifiable();

            _contactService = new ContactService(_mockFileHandler.Object);

            // Invalid email format
            var newContact = new Contact { FirstName = "Jane", LastName = "Smith", Email = "invalid-email" };

            var exception = Assert.Throws<ArgumentException>(() => _contactService.ValidateContact(newContact));
            Assert.Equal("Invalid email format.", exception.Message);
        }

        [Fact]
        public void UpdateContact_ThrowsException_WhenContactNotFound()
        {
            var initialContacts = new List<Contact>
        {
            new Contact { Id = 1, FirstName = "Nishant", LastName = "M", Email = "nishant@example.com" }
        };

            _mockFileHandler.Setup(f => f.Exists(It.IsAny<string>())).Returns(true);
            _mockFileHandler.Setup(f => f.ReadAllText(It.IsAny<string>())).Returns(JsonConvert.SerializeObject(initialContacts));
            _mockFileHandler.Setup(f => f.WriteAllText(It.IsAny<string>(), It.IsAny<string>())).Verifiable();

            _contactService = new ContactService(_mockFileHandler.Object);

            var updatedContact = new Contact { Id = 2, FirstName = "John", LastName = "Doe", Email = "john@example.com" };

            // Try to update a non-existent contact (id = 2)
            var exception = Assert.Throws<KeyNotFoundException>(() => _contactService.UpdateContact(2, updatedContact));
            Assert.Equal("Contact not found.", exception.Message);
        }

        [Fact]
        public void DeleteContact_ThrowsException_WhenContactNotFound()
        {
            var initialContacts = new List<Contact>
        {
            new Contact { Id = 1, FirstName = "Nishant", LastName = "M", Email = "nishant@example.com" }
        };

            _mockFileHandler.Setup(f => f.Exists(It.IsAny<string>())).Returns(true);
            _mockFileHandler.Setup(f => f.ReadAllText(It.IsAny<string>())).Returns(JsonConvert.SerializeObject(initialContacts));
            _mockFileHandler.Setup(f => f.WriteAllText(It.IsAny<string>(), It.IsAny<string>())).Verifiable();

            _contactService = new ContactService(_mockFileHandler.Object);

            // Try to delete a non-existent contact (id = 2)
            var exception = Assert.Throws<KeyNotFoundException>(() => _contactService.DeleteContact(2));
            Assert.Equal("Contact not found.", exception.Message);
        }

        [Fact]
        public void ValidateContact_ThrowsException_WhenMissingFirstName()
        {
            var contact = new Contact { FirstName = "", LastName = "Smith", Email = "jane@example.com" };

            _contactService = new ContactService(_mockFileHandler.Object);

            var exception = Assert.Throws<ArgumentException>(() => _contactService.ValidateContact(contact));
            Assert.Equal("First Name and Last Name are required.", exception.Message);
        }

        [Fact]
        public void ValidateContact_ThrowsException_WhenMissingLastName()
        {
            var contact = new Contact { FirstName = "Jane", LastName = "", Email = "jane@example.com" };

            _contactService = new ContactService(_mockFileHandler.Object);

            var exception = Assert.Throws<ArgumentException>(() => _contactService.ValidateContact(contact));
            Assert.Equal("First Name and Last Name are required.", exception.Message);
        }

        [Fact]
        public void ValidateContact_ThrowsException_WhenEmailIsEmpty()
        {
            var contact = new Contact { FirstName = "Jane", LastName = "Smith", Email = "" };

            _contactService = new ContactService(_mockFileHandler.Object);

            var exception = Assert.Throws<ArgumentException>(() => _contactService.ValidateContact(contact));
            Assert.Equal("Invalid email format.", exception.Message);
        }

        [Fact]
        public void ValidateContact_ThrowsException_WhenEmailFormatIsIncorrect()
        {
            var contact = new Contact { FirstName = "Jane", LastName = "Smith", Email = "invalid-email" };

            _contactService = new ContactService(_mockFileHandler.Object);

            var exception = Assert.Throws<ArgumentException>(() => _contactService.ValidateContact(contact));
            Assert.Equal("Invalid email format.", exception.Message);
        }
    }
}