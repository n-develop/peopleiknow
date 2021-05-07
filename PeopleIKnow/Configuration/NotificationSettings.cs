namespace PeopleIKnow.Configuration
{
    public class NotificationSettings
    {
        public string Token { get; set; }
        public bool Enabled { get; set; }
        public string ChatId { get; set; }
        public TimeSettings Time { get; set; }
    }
}