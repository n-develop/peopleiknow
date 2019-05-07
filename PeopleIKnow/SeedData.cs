using System;
using System.Collections.Generic;
using PeopleIKnow.Models;

namespace PeopleIKnow
{
    internal static class SeedData
    {
        internal static void Initialize(ContactContext db)
        {
            var john = new Contact
            {
                Id = 1,
                Firstname = "John",
                Middlename = "Michael",
                Lastname = "Smith",
                Birthday = new DateTime(1991, 1, 1),
                Address = "Hollywood Boulevard 10, 20099 Hamburg",
                TelephoneNumbers =
                    new List<TelephoneNumber>
                    {
                        new TelephoneNumber {Telephone = "0371 - 555 371987", Type = "Private"}
                    },
                Employer = "Stark Industries",
                BusinessTitle = "CEO",
                Relationships = new List<Relationship>
                {
                    new Relationship {Person = "Jane Smith", Type = "Wife"},
                    new Relationship {Person = "Michael Roger", Type = "Brother"}
                },
                StatusUpdates = new List<StatusUpdate>
                {
                    new StatusUpdate
                    {
                        Created = new DateTime(2018, 1, 1),
                        StatusText =
                            "We met at the Star Bucks in Berlin. We had a nice chat. But he is pretty stressed."
                    },
                    new StatusUpdate
                    {
                        Created = new DateTime(2018, 2, 1),
                        StatusText = "He resigned. Wow. But he is much more relaxed now. Good decision."
                    },
                }
            };
            var jane = new Contact
            {
                Id = 2,
                Firstname = "Jane",
                Middlename = null,
                Lastname = "Smith",
                Birthday = new DateTime(1981, 1, 1),
                Address = "Hollywood Boulevard 10, 20099 Hamburg",
                TelephoneNumbers =
                    new List<TelephoneNumber>
                    {
                        new TelephoneNumber {Telephone = "0371 - 555 371987", Type = "Private"}
                    },
                Employer = "Stark Industries",
                BusinessTitle = "CFO",
                Relationships = new List<Relationship>
                {
                    new Relationship {Person = "John Smith", Type = "Husband"},
                    new Relationship {Person = "Michael Roger", Type = "Brother-in-Law"}
                },
                StatusUpdates = new List<StatusUpdate>
                {
                    new StatusUpdate
                    {
                        Created = new DateTime(2018, 1, 1),
                        StatusText =
                            "We met at the Star Bucks in Berlin. We had a nice chat. But he is pretty stressed."
                    },
                    new StatusUpdate
                    {
                        Created = new DateTime(2018, 2, 1),
                        StatusText = "He resigned. Wow. But he is much more relaxed now. Good decision."
                    },
                }
            };

            var max = new Contact
            {
                Id = 3,
                Firstname = "Max",
                Middlename = null,
                Lastname = "Mustermann",
                Birthday = new DateTime(1981, 2, 21),
                Address = "Musterstr. 39, 38104 Musterstadt",
                TelephoneNumbers =
                    new List<TelephoneNumber>
                    {
                        new TelephoneNumber {Telephone = "04123 - 555 9573", Type = "Private"}
                    },
                Employer = "Crazy Stuff AG",
                BusinessTitle = "Application Developer",
                Relationships = new List<Relationship>
                {
                    new Relationship {Person = "Manuela Mustermann", Type = "Wife"},
                    new Relationship {Person = "Kathrin Mustermann", Type = "Daughter"},
                    new Relationship {Person = "Linus Mustermann", Type = "Son"},
                    new Relationship {Person = "Ronja Baumann", Type = "Sister"},
                    new Relationship {Person = "Moritz Mustermann", Type = "Brother"},
                },
                StatusUpdates = new List<StatusUpdate>
                {
                    new StatusUpdate
                    {
                        Created = new DateTime(2015, 1, 1),
                        StatusText =
                            "Announced, that he will be a father in September. Hooray."
                    },
                    new StatusUpdate
                    {
                        Created = new DateTime(2018, 2, 1),
                        StatusText = "Announced, that he will be a father in September again. Awesome."
                    },
                }
            };

            var contacts = new List<Contact>
            {
                john,
                jane,
                max
            };

            db.Contacts.AddRange(contacts);

            db.SaveChanges();

            var emails = new List<EmailAddress>
            {
                new EmailAddress {Id = 1, Email = "john@smith.com", Type = "Private", ContactId = john.Id},
                new EmailAddress {Id = 2, Email = "jane.smith@webmail.com", Type = "Private", ContactId = jane.Id},
                new EmailAddress {Id = 3, Email = "max@mustermann.example.com", Type = "Private", ContactId = max.Id},
                new EmailAddress {Id = 4, Email = "mustermann.max@crazystuffag.de", Type = "Work", ContactId = max.Id}
            };

            db.EmailAddresses.AddRange(emails);
            db.SaveChanges();
        }
    }
}