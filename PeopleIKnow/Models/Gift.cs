using System.Runtime.Serialization;

namespace PeopleIKnow.Models
{
    public class Gift : IContactProperty
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public GiftType GiftType { get; set; }
        public int ContactId { get; set; }

        [IgnoreDataMember] public virtual Contact Contact { get; set; }
    }
}