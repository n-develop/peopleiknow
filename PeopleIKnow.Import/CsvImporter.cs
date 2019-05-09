using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using CsvHelper;
using PeopleIKnow.Models;

namespace PeopleIKnow.Import
{
    public class CsvImporter
    {
        private readonly ContactContext _db;

        public CsvImporter(ContactContext db)
        {
            _db = db;
        }

        public int ImportFromFile(string filePath)
        {
            var contactCounter = 0;
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader))
            {
                var records = csv.GetRecords<GoogleContact>();
                var nextContactId = _db.Contacts.Max(c => c.Id) + 1;
                var nextMailId = _db.EmailAddresses.Max(c => c.Id) + 1;
                var nextPhoneId = _db.TelephoneNumbers.Max(c => c.Id) + 1;
                var nextStatusUpdateId = _db.StatusUpdates.Max(c => c.Id) + 1;
                var nextRelationshipId = _db.Relationships.Max(c => c.Id) + 1;

                foreach (var googleContact in records)
                {
                    Console.Write($"Importing '{googleContact.Name}'... ");
                    var imagePath = GetImagePath(googleContact, nextContactId);

                    CreateContact(nextContactId, googleContact, imagePath);

                    nextMailId = AddEmails(googleContact, nextContactId, nextMailId);
                    nextStatusUpdateId = AddStatusUpdate(googleContact, nextStatusUpdateId, nextContactId);
                    nextRelationshipId = AddRelationShips(googleContact, nextRelationshipId, nextContactId);
                    nextPhoneId = AddPhoneNumbers(googleContact, nextPhoneId, nextContactId);

                    _db.SaveChanges();
                    nextContactId++;
                    contactCounter++;
                    Console.WriteLine("Done.");
                }
            }

            return contactCounter;
        }

        private void CreateContact(int nextContactId, GoogleContact googleContact, string imagePath)
        {
            var newContact = new Contact
            {
                Id = nextContactId,
                Firstname = googleContact.GivenName,
                Middlename = googleContact.AdditionalName,
                Lastname = googleContact.FamilyName,
                Address = googleContact.Address1Formatted,
                BusinessTitle = googleContact.Organization1Title,
                Employer = googleContact.Organization1Name,
                ImagePath = imagePath
            };
            _db.Contacts.Add(newContact);
        }

        private string GetImagePath(GoogleContact googleContact, int nextContactId)
        {
            var imagePath = "/images/contacts/unknown.jpg";
            if (string.IsNullOrEmpty(googleContact.Photo))
            {
                return imagePath;
            }

            try
            {
                imagePath = SaveImage(googleContact.Photo, nextContactId);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Image '{googleContact.Photo}' could not be downloaded. " + e.Message);
            }

            return imagePath;
        }

        private string SaveImage(string googleContactPhoto, int contactId)
        {
            var relativePath = "../PeopleIKnow/wwwroot/";
            var fileName = $"images/contacts/{contactId}.jpg";
            using (var client = new WebClient())
            {
                client.DownloadFile(new Uri(googleContactPhoto), relativePath + fileName);
            }

            return fileName;
        }

        private int AddPhoneNumbers(GoogleContact googleContact, int nextPhoneId, int nextContactId)
        {
            if (!string.IsNullOrEmpty(googleContact.Phone1Value))
            {
                var phone = new TelephoneNumber
                {
                    Id = nextPhoneId,
                    ContactId = nextContactId,
                    Telephone = googleContact.Phone1Value,
                    Type = googleContact.Phone1Type
                };
                _db.TelephoneNumbers.Add(phone);
                nextPhoneId++;
            }

            if (!string.IsNullOrEmpty(googleContact.Phone2Value))
            {
                var phone = new TelephoneNumber
                {
                    Id = nextPhoneId,
                    ContactId = nextContactId,
                    Telephone = googleContact.Phone2Value,
                    Type = googleContact.Phone2Type
                };
                _db.TelephoneNumbers.Add(phone);
                nextPhoneId++;
            }

            if (!string.IsNullOrEmpty(googleContact.Phone3Value))
            {
                var phone = new TelephoneNumber
                {
                    Id = nextPhoneId,
                    ContactId = nextContactId,
                    Telephone = googleContact.Phone3Value,
                    Type = googleContact.Phone3Type
                };
                _db.TelephoneNumbers.Add(phone);
                nextPhoneId++;
            }

            return nextPhoneId;
        }

        private int AddRelationShips(GoogleContact googleContact, int nextRelationshipId, int nextContactId)
        {
            if (!string.IsNullOrEmpty(googleContact.Relation1Value))
            {
                var relationShip = new Relationship
                {
                    Id = nextRelationshipId,
                    ContactId = nextContactId,
                    Person = googleContact.Relation1Value,
                    Type = googleContact.Relation1Type
                };
                _db.Relationships.Add(relationShip);
                nextRelationshipId++;
            }

            if (!string.IsNullOrEmpty(googleContact.Relation2Value))
            {
                var relationShip = new Relationship
                {
                    Id = nextRelationshipId,
                    ContactId = nextContactId,
                    Person = googleContact.Relation2Value,
                    Type = googleContact.Relation2Type
                };
                _db.Relationships.Add(relationShip);
                nextRelationshipId++;
            }

            if (!string.IsNullOrEmpty(googleContact.Relation3Value))
            {
                var relationShip = new Relationship
                {
                    Id = nextRelationshipId,
                    ContactId = nextContactId,
                    Person = googleContact.Relation3Value,
                    Type = googleContact.Relation3Type
                };
                _db.Relationships.Add(relationShip);
                nextRelationshipId++;
            }

            if (!string.IsNullOrEmpty(googleContact.Relation4Value))
            {
                var relationShip = new Relationship
                {
                    Id = nextRelationshipId,
                    ContactId = nextContactId,
                    Person = googleContact.Relation4Value,
                    Type = googleContact.Relation4Type
                };
                _db.Relationships.Add(relationShip);
                nextRelationshipId++;
            }

            return nextRelationshipId;
        }

        private int AddStatusUpdate(GoogleContact googleContact, int nextStatusUpdateId, int nextContactId)
        {
            if (!string.IsNullOrEmpty(googleContact.Notes))
            {
                var statusUpdate = new StatusUpdate
                {
                    Id = nextStatusUpdateId,
                    Created = DateTime.Now,
                    ContactId = nextContactId,
                    StatusText = googleContact.Notes
                };
                _db.StatusUpdates.Add(statusUpdate);
                nextStatusUpdateId++;
            }

            return nextStatusUpdateId;
        }

        private int AddEmails(GoogleContact googleContact, int nextContactId, int nextMailId)
        {
            if (!string.IsNullOrEmpty(googleContact.Email1Value))
            {
                var mail1 = new EmailAddress
                {
                    Id = nextMailId,
                    ContactId = nextContactId,
                    Email = googleContact.Email1Value,
                    Type = googleContact.Email1Type
                };
                _db.EmailAddresses.Add(mail1);
                nextMailId++;
            }

            if (!string.IsNullOrEmpty(googleContact.Email2Value))
            {
                var mail2 = new EmailAddress
                {
                    Id = nextMailId,
                    ContactId = nextContactId,
                    Email = googleContact.Email2Value,
                    Type = googleContact.Email2Type
                };
                _db.EmailAddresses.Add(mail2);
                nextMailId++;
            }

            if (!string.IsNullOrEmpty(googleContact.Email3Value))
            {
                var mail3 = new EmailAddress
                {
                    Id = nextMailId,
                    ContactId = nextContactId,
                    Email = googleContact.Email3Value,
                    Type = googleContact.Email3Type
                };
                _db.EmailAddresses.Add(mail3);
                nextMailId++;
            }

            return nextMailId;
        }
    }
}