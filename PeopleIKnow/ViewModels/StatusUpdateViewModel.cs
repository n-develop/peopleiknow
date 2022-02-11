using System;

namespace PeopleIKnow.ViewModels
{
    public class StatusUpdateViewModel
    {
        public int Id { get; set; }
        public string FormattedStatusText { get; set; }
        public DateTime Created { get; set; }

        public int ContactId { get; set; }
    }
}