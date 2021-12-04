using System;

namespace PeopleIKnow.Models
{
    public class BirthdayContact
    {
        public string FullName { get; set; }
        public DateTime Birthday { get; set; }
        public bool BirthdayToday { get; set; }
    }
}