using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using PeopleIKnow.Models;

namespace PeopleIKnow.Repositories
{
    public interface IContactRepository
    {
        Task<IEnumerable<Contact>> GetAllContacts();
        Task<IEnumerable<Contact>> GetContacts(int skip, int take);
        Contact GetContactById(int id);
        bool DeleteContact(int id);
        Contact AddContact(Contact contact);
        bool SaveContact(Contact contact);
        void AddEmail(EmailAddress mail);
        EmailAddress GetEmailById(int mailId);
        void DeleteEmailAddress(EmailAddress mail);
        void AddStatusUpdate(StatusUpdate statusUpdate);
        void AddTelephoneNumber(TelephoneNumber telephoneNumber);
        TelephoneNumber GetTelephoneNumberById(int id);
        void DeleteTelephoneNumber(TelephoneNumber telephoneNumber);
        void AddRelationship(Relationship relationship);
        Relationship GetRelationshipById(int id);
        void DeleteRelationship(Relationship relationship);
        void UpdateEmail(EmailAddress mail);
    }
}