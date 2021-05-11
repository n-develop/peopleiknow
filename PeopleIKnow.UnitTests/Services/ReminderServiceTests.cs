using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NSubstitute;
using PeopleIKnow.DataAccess.Repositories;
using PeopleIKnow.Models;
using PeopleIKnow.Services;
using Xunit;

namespace PeopleIKnow.UnitTests.Services
{
    public class ReminderServiceTests
    {
        private ReminderService _sut;
        private IMessagingService _messagingService;
        private IRepository<Reminder> _reminderRepository;
        private IContactRepository _contactRepository;

        public ReminderServiceTests()
        {
            _messagingService = Substitute.For<IMessagingService>();
            _reminderRepository = Substitute.For<IRepository<Reminder>>();
            _contactRepository = Substitute.For<IContactRepository>();
            _sut = new ReminderService(_messagingService, _reminderRepository, _contactRepository);
        }

        [Fact]
        public async Task NoReminderFound_NoReminderSent()
        {
            // Arrange
            // Act
            await _sut.SendReminders(DateTime.Today);

            // Assert
            await _messagingService.DidNotReceiveWithAnyArgs().SendMessageAsync(null, null);
        }

        [Fact]
        public async Task OneReminderWithSameYearFound_ReminderSentWithCorrectMessage()
        {
            // Arrange
            _reminderRepository.GetAll().Returns(new List<Reminder>
            {
                new Reminder
                {
                    Contact = new Contact
                    {
                        Firstname = "John",
                        Middlename = "Jonas",
                        Lastname = "Smith"
                    },
                    Date = new DateTime(2021, 02, 01),
                    Description = "Wedding",
                    RemindMeEveryYear = true
                }
            }.AsQueryable());

            // Act
            await _sut.SendReminders(new DateTime(2021, 02, 01));

            // Assert
            await _messagingService.Received(1).SendMessageAsync("⏰ Wedding", "Wedding reminder for John Jonas Smith");
        }

        [Fact]
        public async Task OneReminderWithDifferentYearFound_ReminderSentWithCorrectMessage()
        {
            // Arrange
            _reminderRepository.GetAll().Returns(new List<Reminder>
            {
                new Reminder
                {
                    Contact = new Contact
                    {
                        Firstname = "Anne",
                        Lastname = "Johnson"
                    },
                    Date = new DateTime(2019, 02, 01),
                    Description = "Exam",
                    RemindMeEveryYear = true
                }
            }.AsQueryable());

            // Act
            await _sut.SendReminders(new DateTime(2021, 02, 01));

            // Assert
            await _messagingService.Received(1).SendMessageAsync("⏰ Exam", "Exam reminder for Anne Johnson");
        }

        [Fact]
        public async Task OneNonRecurringReminderWithDifferentYearFound_ReminderNotSent()
        {
            // Arrange
            _reminderRepository.GetAll().Returns(new List<Reminder>
            {
                new Reminder
                {
                    Contact = new Contact
                    {
                        Firstname = "Max",
                        Lastname = "Miller"
                    },
                    Date = new DateTime(2019, 02, 01),
                    Description = "Stuff",
                    RemindMeEveryYear = false
                }
            }.AsQueryable());

            // Act
            await _sut.SendReminders(new DateTime(2021, 02, 01));

            // Assert
            await _messagingService.DidNotReceiveWithAnyArgs().SendMessageAsync(null, null);
        }


        [Fact]
        public async Task OneBirthdayFound_ReminderSentWithCorrectMessage()
        {
            // Arrange
            _contactRepository.GetBirthdayContactsAsync(Arg.Any<DateTime>())
                .Returns(new List<Contact>
                {
                    new Contact
                    {
                        Firstname = "Joseph",
                        Lastname = "Jasper",
                        Birthday = new DateTime(1990, 9, 21)
                    }
                });

            // Act
            await _sut.SendReminders(new DateTime(2021, 9, 21));

            // Assert
            await _messagingService.Received(1).SendMessageAsync("🎁 Joseph Jasper", "It's Joseph Jasper's birthday");
        }
    }
}