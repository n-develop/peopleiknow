using System.Runtime.Serialization;

namespace PeopleIKnow.Models
{
    public class EmailAddress : IContactProperty
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Type { get; set; }

        public int ContactId { get; set; }

        [IgnoreDataMember] public virtual Contact Contact { get; set; }
    }
}