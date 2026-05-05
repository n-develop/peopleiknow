using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FolkerKinzel.VCards;
using FolkerKinzel.VCards.Enums;
using FolkerKinzel.VCards.Models;
using PeopleIKnow.Models;

namespace PeopleIKnow.CardDavExport
{
    public class ContactToVCardMapper
    {
        private readonly string _webRootPath;

        public ContactToVCardMapper(string webRootPath)
        {
            _webRootPath = webRootPath;
        }

        public VCard Map(Contact contact)
        {
            var vcard = new VCard();

            // UID - deterministic based on contact ID
            vcard.ID = new IDProperty(GenerateDeterministicGuid(contact.Id));

            // N - structured name
            vcard.NameViews = new[]
            {
                new NameProperty(
                    familyName: contact.Lastname,
                    givenName: contact.Firstname,
                    additionalName: contact.Middlename)
            };

            // FN - formatted name
            var fullName = contact.FullName;
            if (!string.IsNullOrWhiteSpace(fullName))
            {
                vcard.DisplayNames = new[] { new TextProperty(fullName) };
            }

            // BDAY - birthday (skip if default/min value)
            if (contact.Birthday != DateTime.MinValue && contact.Birthday.Year > 1)
            {
                vcard.BirthDayViews = new[]
                {
                    DateAndOrTimeProperty.FromDate(
                        contact.Birthday.Year,
                        contact.Birthday.Month,
                        contact.Birthday.Day)
                };
            }

            // ADR - address (single string goes into street component)
            if (!string.IsNullOrWhiteSpace(contact.Address))
            {
                vcard.Addresses = new[]
                {
                    new AddressProperty(street: contact.Address, locality: null,
                        region: null, postalCode: null, country: null)
                };
            }

            // TEL - telephone numbers
            if (contact.TelephoneNumbers?.Any() == true)
            {
                var phones = new List<TextProperty>();
                foreach (var tel in contact.TelephoneNumbers)
                {
                    var prop = new TextProperty(tel.Telephone);
                    prop.Parameters.PhoneType = MapPhoneType(tel.Type);
                    prop.Parameters.PropertyClass = MapPropertyClass(tel.Type);
                    phones.Add(prop);
                }
                vcard.Phones = phones;
            }

            // EMAIL - email addresses
            if (contact.EmailAddresses?.Any() == true)
            {
                var emails = new List<TextProperty>();
                foreach (var email in contact.EmailAddresses)
                {
                    var prop = new TextProperty(email.Email);
                    prop.Parameters.PropertyClass = MapPropertyClass(email.Type);
                    emails.Add(prop);
                }
                vcard.EMails = emails;
            }

            // ORG - organization
            if (!string.IsNullOrWhiteSpace(contact.Employer))
            {
                vcard.Organizations = new[] { new OrgProperty(contact.Employer) };
            }

            // TITLE - business title
            if (!string.IsNullOrWhiteSpace(contact.BusinessTitle))
            {
                vcard.Titles = new[] { new TextProperty(contact.BusinessTitle) };
            }

            // PHOTO - contact image
            if (!string.IsNullOrWhiteSpace(contact.ImagePath))
            {
                var imagePath = Path.Combine(_webRootPath, contact.ImagePath.TrimStart('/'));
                if (File.Exists(imagePath))
                {
                    vcard.Photos = new[] { DataProperty.FromFile(imagePath) };
                }
            }

            // CATEGORIES - tags
            if (!string.IsNullOrWhiteSpace(contact.Tags))
            {
                var tags = contact.Tags.Split(',')
                    .Select(t => t.Trim())
                    .Where(t => !string.IsNullOrEmpty(t));
                vcard.Categories = new[] { new StringCollectionProperty(tags) };
            }

            // RELATED - relationships
            if (contact.Relationships?.Any() == true)
            {
                var relations = new List<RelationProperty>();
                foreach (var rel in contact.Relationships)
                {
                    var relType = MapRelationType(rel.Type);
                    relations.Add(RelationProperty.FromText(rel.Person, relType));
                }
                vcard.Relations = relations;
            }

            // NOTE - status updates
            if (contact.StatusUpdates?.Any() == true)
            {
                var statusLines = contact.StatusUpdates
                    .OrderByDescending(s => s.Created)
                    .Select(s => $"[Status {s.Created:yyyy-MM-dd}: {s.StatusText}]");
                var noteText = string.Join("\n", statusLines);
                vcard.Notes = new[] { new TextProperty(noteText) };
            }

            return vcard;
        }

        private static Tel? MapPhoneType(string type)
        {
            if (string.IsNullOrWhiteSpace(type)) return Tel.Voice;

            return type.ToLowerInvariant() switch
            {
                "mobile" or "cell" => Tel.Cell,
                "fax" => Tel.Fax,
                "pager" => Tel.Pager,
                "video" => Tel.Video,
                _ => Tel.Voice
            };
        }

        private static PCl? MapPropertyClass(string type)
        {
            if (string.IsNullOrWhiteSpace(type)) return null;

            return type.ToLowerInvariant() switch
            {
                "work" or "business" => PCl.Work,
                "home" or "personal" or "private" => PCl.Home,
                _ => null
            };
        }

        private static Rel? MapRelationType(string type)
        {
            if (string.IsNullOrWhiteSpace(type)) return null;

            return type.ToLowerInvariant() switch
            {
                "parent" or "mother" or "father" => Rel.Parent,
                "child" or "son" or "daughter" => Rel.Child,
                "sibling" or "brother" or "sister" => Rel.Sibling,
                "spouse" or "husband" or "wife" or "partner" => Rel.Spouse,
                "friend" => Rel.Friend,
                "colleague" or "coworker" or "co-worker" => Rel.Colleague,
                "neighbor" or "neighbour" => Rel.Neighbor,
                _ => Rel.Acquaintance
            };
        }

        private static Guid GenerateDeterministicGuid(int contactId)
        {
            // Create a deterministic GUID from the contact ID
            // Using a fixed namespace to ensure consistency across runs
            var bytes = new byte[16];
            var idBytes = BitConverter.GetBytes(contactId);
            // Fixed namespace prefix: "pik-contact-"
            bytes[0] = 0x70; // 'p'
            bytes[1] = 0x69; // 'i'
            bytes[2] = 0x6B; // 'k'
            bytes[3] = 0x2D; // '-'
            Array.Copy(idBytes, 0, bytes, 12, 4);
            return new Guid(bytes);
        }
    }
}
