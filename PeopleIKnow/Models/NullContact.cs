namespace PeopleIKnow.Models
{
    public class NullContact : Contact
    {
        private static NullContact _instance;

        private NullContact()
        {
        }

        public static NullContact GetInstance()
        {
            if (_instance == null)
            {
                _instance = new NullContact();
            }

            return _instance;
        }

        public override bool IsNull()
        {
            return true;
        }
    }
}