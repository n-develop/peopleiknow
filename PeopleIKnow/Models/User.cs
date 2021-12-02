namespace PeopleIKnow.Models
{
    public class User
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsUser { get; set; }
    }
}