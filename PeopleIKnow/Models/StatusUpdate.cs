using System;
using System.Runtime.Serialization;

namespace PeopleIKnow.Models
{
    public class StatusUpdate
    {
        public int Id { get; set; }
        public string StatusText { get; set; }
        public DateTime Created { get; set; }

        public int ContactId { get; set; }

        [IgnoreDataMember] public virtual Contact Contact { get; set; }

        public virtual bool IsNull()
        {
            return false;
        }
    }
}