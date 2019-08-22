using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.Core;
using PeopleIKnow.Controllers;
using PeopleIKnow.Models;
using PeopleIKnow.Repositories;
using Xunit;

namespace PeopleIKnow.UnitTests.ControllerTests.SearchControllerTests
{
    public class IndexTests
    {
        #region test infrastructure

        private IContactRepository _contactRepository;

        public IndexTests()
        {
            _contactRepository = Substitute.For<IContactRepository>();
        }

        private SearchController CreateController()
        {
            return new SearchController(_contactRepository);
        }

        #endregion

        [Fact]
        public async Task NoSearchTerm_ReturnsRedirect()
        {
            // Arrange
            var controller = CreateController();

            // Act
            var actionResult = await controller.Index("");

            // Assert
            actionResult.Should().BeOfType<RedirectToActionResult>();
        }

        [Fact]
        public async Task NoSearchTerm_RedirectsToContactList()
        {
            // Arrange
            var controller = CreateController();

            // Act
            var redirectToActionResult = await controller.Index("") as RedirectToActionResult;

            // Assert
            redirectToActionResult.ActionName.Should().Be("ContactList");
            redirectToActionResult.ControllerName.Should().Be("Dashboard");
        }

        [Fact]
        public void ValidSearchTerm_ReturnsView()
        {
            // Arrange
            var contacts = new List<Contact>
            {
                new Contact
                {
                    Lastname = "AAA",
                    IsFavorite = false
                },
                new Contact
                {
                    Lastname = "ZZZ",
                    IsFavorite = true
                },
                new Contact
                {
                    Lastname = "XXX",
                    IsFavorite = false
                },
                new Contact
                {
                    Lastname = "AAB",
                    IsFavorite = true
                },
            };
            _contactRepository.SearchContacts(Arg.Any<string>()).Returns(contacts);

            // Act

            // Assert
            throw new NotImplementedException();
        }
    }
}