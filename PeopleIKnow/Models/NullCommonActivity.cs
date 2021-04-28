using System;

namespace PeopleIKnow.Models
{
    public class NullCommonActivity : CommonActivity
    {
        private static NullCommonActivity _instance;

        private NullCommonActivity()
        {
            Description = "[Not Found]";
            Date = DateTime.MinValue;
        }

        public static NullCommonActivity GetInstance()
        {
            if (_instance == null)
            {
                _instance = new NullCommonActivity();
            }

            return _instance;
        }

        public override bool IsNull()
        {
            return true;
        }
    }
}