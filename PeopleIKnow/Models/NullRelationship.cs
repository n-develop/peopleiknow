namespace PeopleIKnow.Models
{
    class NullRelationship : Relationship
    {
        private static NullRelationship _instance;

        private NullRelationship()
        {
            Person = "[Not Found]";
            Type = "[Not Found]";
        }

        public static NullRelationship GetInstance()
        {
            if (_instance == null)
            {
                _instance = new NullRelationship();
            }

            return _instance;
        }

        public override bool IsNull()
        {
            return true;
        }
    }
}