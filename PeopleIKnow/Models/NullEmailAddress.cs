namespace PeopleIKnow.Models
{
    public class NullEmailAddress : EmailAddress
    {
        private static NullEmailAddress _instance;

        private NullEmailAddress()
        {
            Email = "[Not Found]";
            Type = "[Not Found]";
        }

        public static NullEmailAddress GetInstance()
        {
            if (_instance == null)
            {
                _instance = new NullEmailAddress();
            }

            return _instance;
        }

        public override bool IsNull()
        {
            return true;
        }
    }
}