using System;
using Microsoft.AspNetCore.Http;

namespace PeopleIKnow.ViewModels
{
    public class ContactUpdateViewModel
    {
        public int Id { get; set; }

        public bool IsFavorite { get; set; }

        public string Firstname { get; set; }

        public string Middlename { get; set; }

        public string Lastname { get; set; }

        public IFormFile Image { get; set; }

        public DateTime Birthday { get; set; }

        public string Address { get; set; }

        public string Employer { get; set; }

        public string BusinessTitle { get; set; }

        public string Tags { get; set; }
    }
}