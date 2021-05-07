using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.Core;
using PeopleIKnow.Controllers;
using PeopleIKnow.DataAccess.Repositories;
using PeopleIKnow.Models;
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
        public async Task ValidSearchTerm_ReturnsView()
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
            var controller = CreateController();

            // Act
            var searchResult = await controller.Index("AAB");

            // Assert
            searchResult.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public async Task ValidSearchTerm_ReturnsContactListView()
        {
            // Arrange
            var controller = CreateController();

            // Act
            var searchResult = await controller.Index("Max") as ViewResult;

            // Assert
            searchResult.ViewName.Should().Be("Components/ContactList/Default");
        }

        [Fact]
        public async Task ValidSearchTerm_ReturnsContactsInModel()
        {
            // Arrange
            var contacts = new List<Contact>
            {
                new Contact
                {
                    Lastname = "AAA",
                    IsFavorite = false
                }
            };
            _contactRepository.SearchContacts(Arg.Any<string>()).Returns(contacts);
            var controller = CreateController();

            // Act
            var searchResult = await controller.Index("AAA") as ViewResult;

            // Assert
            searchResult.Model.Should().BeOfType<List<Contact>>();
            var model = searchResult.Model as List<Contact>;
            model.Count.Should().Be(1);
            model[0].Lastname.Should().Be("AAA");
        }
    }
}