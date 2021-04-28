using Microsoft.EntityFrameworkCore;
using PeopleIKnow.Models;

namespace PeopleIKnow
{
    public class ContactContext : DbContext
    {
        public ContactContext()
        {
        }

        public ContactContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Contact> Contacts { get; set; }

        public DbSet<EmailAddress> EmailAddresses { get; set; }

        public DbSet<Relationship> Relationships { get; set; }

        public DbSet<StatusUpdate> StatusUpdates { get; set; }

        public DbSet<TelephoneNumber> TelephoneNumbers { get; set; }

        public DbSet<CommonActivity> CommonActivities { get; set; }
    }
}