using ContactsMangementAPI.Models;

namespace ContactsMangementAPI.Services
{
    public interface IContactService
    {
        Contact GetContactById(int id);
        List<Contact> GetContacts();
        void CreateContact(Contact contact);
        void UpdateContact(int id, Contact contact);
        void DeleteContact(int id);
        bool ValidateContact(Contact contact);
    }
}
