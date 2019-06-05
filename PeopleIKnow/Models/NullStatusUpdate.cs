using System;

namespace PeopleIKnow.Models
{
    public class NullStatusUpdate : StatusUpdate
    {
        private static NullStatusUpdate _instance;

        private NullStatusUpdate()
        {
            StatusText = "[Not Found]";
            Created = DateTime.MinValue;
        }

        public static NullStatusUpdate GetInstance()
        {
            if (_instance == null)
            {
                _instance = new NullStatusUpdate();
            }

            return _instance;
        }

        public override bool IsNull()
        {
            return true;
        }
    }
}