using System.Runtime.Serialization;

namespace PeopleIKnow.Models
{
    public class TelephoneNumber
    {
        public int Id { get; set; }
        public string Telephone { get; set; }
        public string Type { get; set; }

        public int ContactId { get; set; }

        [IgnoreDataMember] public virtual Contact Contact { get; set; }

        public virtual bool IsNull()
        {
            return false;
        }
    }
}