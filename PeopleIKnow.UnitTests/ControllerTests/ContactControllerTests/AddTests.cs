using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using PeopleIKnow.Models;
using Xunit;

namespace PeopleIKnow.UnitTests.ControllerTests.ContactControllerTests
{
    public class AddTests : TestBase
    {
        [Fact]
        public void Get_ReturnsView()
        {
            // Arrange
            var controller = CreateController();

            // Act
            var actionResult = controller.Add();

            // Assert
            actionResult.Should().NotBeNull();
            actionResult.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public async Task Post_ReceivesContactWithId_ReturnsBadRequest()
        {
            // Arrange
            var controller = CreateController();

            // Act
            var actionResult = await controller.Add(new Contact {Id = 3});

            // Assert
            actionResult.Should().NotBeNull();
            actionResult.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task Post_ContactCouldNotBeCreated_ReturnsBadRequest()
        {
            // Arrange
            _contactRepository.AddContact(Arg.Any<Contact>()).Returns(NullContact.GetInstance());
            var controller = CreateController();

            // Act
            var actionResult = await controller.Add(new Contact());

            // Assert
            actionResult.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task Post_ContactWasCreated_ReturnsRedirect()
        {
            // Arrange
            _contactRepository.AddContact(Arg.Any<Contact>()).Returns(new Contact {Id = 1});
            var controller = CreateController();

            // Act
            var actionResult = await controller.Add(new Contact());

            // Assert
            actionResult.Should().BeOfType<RedirectToActionResult>();
        }

        [Fact]
        public async Task Post_ContactWasCreated_ReturnsRedirectToDetails()
        {
            // Arrange
            _contactRepository.AddContact(Arg.Any<Contact>()).Returns(new Contact {Id = 1});
            var controller = CreateController();

            // Act
            var actionResult = await controller.Add(new Contact());

            // Assert
            actionResult.Should().BeOfType<RedirectToActionResult>();
            var result = actionResult as RedirectToActionResult;
            result.Should().NotBeNull();
            result.ActionName.Should().Be("Details");
            result.RouteValues.Should().ContainKey("id");
            result.RouteValues["id"].Should().Be(1);
        }
    }
}