using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace PeopleIKnow.Models
{
    public class Contact
    {
        public int Id { get; set; }

        public bool IsFavorite { get; set; }

        public string Firstname { get; set; }

        public string Middlename { get; set; }

        public string Lastname { get; set; }

        public string ImagePath { get; set; }

        public DateTime Birthday { get; set; }

        public string Address { get; set; }

        public List<TelephoneNumber> TelephoneNumbers { get; set; }

        public List<EmailAddress> EmailAddresses { get; set; }

        public string Employer { get; set; }

        public string BusinessTitle { get; set; }

        public List<Relationship> Relationships { get; set; }

        public List<StatusUpdate> StatusUpdates { get; set; }

        public List<CommonActivity> Activities { get; set; }

        public List<Gift> Gifts { get; set; }

        public List<Reminder> Reminders { get; set; }

        public string Tags { get; set; }

        [NotMapped]
        public List<StatusUpdate> StatusTimeline
        {
            get
            {
                if (StatusUpdates != null && StatusUpdates.Any())
                {
                    return StatusUpdates.OrderByDescending(c => c.Created).ToList();
                }

                return new List<StatusUpdate>();
            }
        }
        
        [NotMapped]
        public List<CommonActivity> ActivityTimeline
        {
            get
            {
                if (Activities != null && Activities.Any())
                {
                    return Activities.OrderByDescending(c => c.Date).ToList();
                }

                return new List<CommonActivity>();
            }
        }

        [NotMapped]
        public string FullName
        {
            get
            {
                var fullname = string.Empty;
                if (!string.IsNullOrWhiteSpace(Firstname))
                {
                    fullname = Firstname;
                }

                if (!string.IsNullOrWhiteSpace(Middlename))
                {
                    fullname = string.Join(" ", fullname, Middlename);
                }

                if (!string.IsNullOrWhiteSpace(Lastname))
                {
                    fullname = string.Join(" ", fullname, Lastname);
                }

                return fullname.Trim();
            }
        }

        public virtual bool IsNull()
        {
            return false;
        }
    }
}