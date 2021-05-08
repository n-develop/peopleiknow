using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NSubstitute;
using PeopleIKnow.DataAccess;
using PeopleIKnow.DataAccess.Repositories;
using PeopleIKnow.Models;
using Xunit;

namespace PeopleIKnow.UnitTests.DataAccess.Repositories
{
    public class ContactRepositoryTests : IDisposable
    {
        #region test infrastructure

        private IContactRepository _sut;
        private ContactContext _context;

        public ContactRepositoryTests()
        {
            var contextOptions = new DbContextOptionsBuilder<ContactContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new ContactContext(contextOptions);
            _sut = new ContactRepository(_context, Substitute.For<ILogger<ContactRepository>>());
        }

        public void Dispose()
        {
            _context?.Database?.EnsureDeleted();
            _context?.Dispose();
        }

        #endregion

        [Fact]
        public async Task NoBirthdaysForTheGivenDay_ReturnsNoContacts()
        {
            // Arrange
            await _context.Contacts.AddAsync(new Contact
            {
                Id = 1,
                Birthday = new DateTime(1990, 4, 1)
            });
            await _context.Contacts.AddAsync(new Contact
            {
                Id = 2,
                Birthday = new DateTime(1990, 4, 3)
            });
            await _context.SaveChangesAsync();

            // Act
            var contacts = await _sut.GetBirthdayContactsAsync(new DateTime(1990, 4, 2));

            // Assert
            contacts.Should().BeEmpty();
        }

        [Fact]
        public async Task OneBirthdayForTheGivenDay_ReturnsOneContacts()
        {
            // Arrange
            await _context.Contacts.AddAsync(new Contact
            {
                Id = 1,
                Birthday = new DateTime(1990, 5, 1)
            });
            await _context.Contacts.AddAsync(new Contact
            {
                Id = 2,
                Birthday = new DateTime(1990, 5, 2)
            });
            await _context.Contacts.AddAsync(new Contact
            {
                Id = 3,
                Birthday = new DateTime(1990, 5, 3)
            });
            await _context.SaveChangesAsync();

            // Act
            var contacts = await _sut.GetBirthdayContactsAsync(new DateTime(1990, 5, 3));

            // Assert
            contacts.Should().HaveCount(1);
        }

        [Fact]
        public async Task OneBirthdayInDifferentYearForTheGivenDay_ReturnsOneContacts()
        {
            // Arrange
            await _context.Contacts.AddAsync(new Contact
            {
                Id = 1,
                Birthday = new DateTime(1990, 5, 1)
            });
            await _context.Contacts.AddAsync(new Contact
            {
                Id = 2,
                Birthday = new DateTime(1990, 5, 2)
            });
            await _context.Contacts.AddAsync(new Contact
            {
                Id = 3,
                Birthday = new DateTime(1990, 5, 3)
            });
            await _context.SaveChangesAsync();

            // Act
            var contacts = await _sut.GetBirthdayContactsAsync(new DateTime(2021, 5, 3));

            // Assert
            contacts.Should().HaveCount(1);
            contacts.First().Id.Should().Be(3);
        }
    }
}