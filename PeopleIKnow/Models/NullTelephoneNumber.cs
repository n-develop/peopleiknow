namespace PeopleIKnow.Models
{
    public class NullTelephoneNumber : TelephoneNumber
    {
        private static NullTelephoneNumber _instance;

        private NullTelephoneNumber()
        {
            Telephone = "[Not Found]";
            Type = "[Not Found]";
        }

        public static NullTelephoneNumber GetInstance()
        {
            if (_instance == null)
            {
                _instance = new NullTelephoneNumber();
            }

            return _instance;
        }

        public override bool IsNull()
        {
            return true;
        }
    }
}