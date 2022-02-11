using System;
using System.Runtime.Serialization;

namespace PeopleIKnow.Models
{
    public class Reminder : IContactProperty
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public bool RemindMeEveryYear { get; set; }

        public int ContactId { get; set; }

        [IgnoreDataMember] public virtual Contact Contact { get; set; }
    }
}