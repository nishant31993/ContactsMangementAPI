﻿using ContactsMangementAPI.Handlers;
using ContactsMangementAPI.Models;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace ContactsMangementAPI.Services
{
    public class ContactService : IContactService
    {
        private readonly IFileHandler _fileHandler;
        private const string FilePath = "contacts.json";
        private List<Contact> contacts;

        public ContactService(IFileHandler fileHandler)
        {
            _fileHandler = fileHandler;
            if (_fileHandler.Exists(FilePath))
            {
                var json = _fileHandler.ReadAllText(FilePath);
                contacts = JsonConvert.DeserializeObject<List<Contact>>(json) ?? new List<Contact>();
            }
            else
            {
                contacts = new List<Contact>();
            }
        }

        private void SaveChanges()
        {
            var json = JsonConvert.SerializeObject(contacts, Formatting.Indented);
            _fileHandler.WriteAllText(FilePath, json);
        }

        public List<Contact> GetContacts() => contacts;

        public Contact GetContactById(int id) => contacts.FirstOrDefault(c => c.Id == id);

        public void CreateContact(Contact contact)
        {
            if (contacts.Any(c => c.Email == contact.Email))
                throw new InvalidOperationException("Email must be unique.");

            // If the contacts list is empty, set the ID to 1, otherwise assign the next ID.
            contact.Id = contacts.Any() ? contacts.Max(c => c.Id) + 1 : 1;

            contacts.Add(contact);
            SaveChanges();
        }

        public void UpdateContact(int id, Contact updatedContact)
        {
            var existingContact = GetContactById(id);
            if (existingContact == null)
                throw new KeyNotFoundException("Contact not found.");

            if (contacts.Any(c => c.Email == updatedContact.Email && c.Id != id))
                throw new InvalidOperationException("Email must be unique.");

            existingContact.FirstName = updatedContact.FirstName;
            existingContact.LastName = updatedContact.LastName;
            existingContact.Email = updatedContact.Email;
            SaveChanges();
        }

        public void DeleteContact(int id)
        {
            var contact = GetContactById(id);
            if (contact == null)
                throw new KeyNotFoundException("Contact not found.");

            contacts.Remove(contact);
            SaveChanges();
        }

        public bool ValidateContact(Contact contact)
        {
            if (string.IsNullOrWhiteSpace(contact.FirstName) || string.IsNullOrWhiteSpace(contact.LastName))
                throw new ArgumentException("First Name and Last Name are required.");

            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            if (!emailRegex.IsMatch(contact.Email))
                throw new ArgumentException("Invalid email format.");

            return true;
        }
    }
}
