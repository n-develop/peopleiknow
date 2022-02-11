namespace PeopleIKnow.Models
{
    public interface IContactProperty
    {
        public int Id { get; set; }
        public int ContactId { get; set; }

        public Contact Contact { get; set; }
    }
}