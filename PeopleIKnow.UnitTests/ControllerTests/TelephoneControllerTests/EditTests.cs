using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using PeopleIKnow.Models;
using Xunit;

namespace PeopleIKnow.UnitTests.ControllerTests.TelephoneControllerTests
{
    public class EditTests : BaseTelephoneControllerTests
    {
        [Fact]
        public void Get_ReceivesInvalidId_ReturnsNotFound()
        {
            // Arrange
            var controller = CreateController();

            // Act
            var actionResult = controller.Edit(0);

            // Assert
            actionResult.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void Get_CannotFindContactById_ReturnsNotFound()
        {
            // Arrange
            _contactRepository.GetTelephoneNumberById(2).Returns(NullTelephoneNumber.GetInstance());
            var controller = CreateController();

            // Act
            var actionResult = controller.Edit(2);

            // Assert
            actionResult.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void Get_ReceivesValidId_ReturnsView()
        {
            // Arrange
            _contactRepository.GetTelephoneNumberById(2).Returns(new TelephoneNumber {Id = 2});
            var controller = CreateController();

            // Act
            var actionResult = controller.Edit(2);

            // Assert
            actionResult.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public void Get_ReceivesValidId_ReturnsModelWithId()
        {
            // Arrange
            _contactRepository.GetTelephoneNumberById(2).Returns(new TelephoneNumber {Id = 2});
            var controller = CreateController();

            // Act
            var actionResult = controller.Edit(2);

            // Assert
            var model = (actionResult as ViewResult).Model;
            model.Should().BeOfType<TelephoneNumber>();
            (model as TelephoneNumber).Id.Should().Be(2);
        }

        [Fact]
        public void Post_ReceivesNullAsModel_ReturnsBadRequest()
        {
            // Arrange
            var controller = CreateController();

            // Act
            var actionResult = controller.Edit(null);

            // Assert
            actionResult.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public void Post_ReceivesValidMode_ReturnsRedirect()
        {
            // Arrange
            var controller = CreateController();

            // Act
            var actionResult = controller.Edit(new TelephoneNumber());

            // Assert
            actionResult.Should().BeOfType<RedirectToActionResult>();
        }

        [Fact]
        public void Post_ReceivesValidModel_ReturnsRedirectToDetails()
        {
            // Arrange
            var controller = CreateController();

            // Act
            var actionResult = controller.Edit(new TelephoneNumber()) as RedirectToActionResult;

            // Assert
            actionResult.ActionName.Should().Be("Details");
            actionResult.ControllerName.Should().Be("Dashboard");
        }
    }
}