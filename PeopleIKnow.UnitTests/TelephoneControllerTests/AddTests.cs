using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using PeopleIKnow.Models;
using Xunit;

namespace PeopleIKnow.UnitTests.TelephoneControllerTests
{
    public class AddTests : BaseTelephoneControllerTests
    {
        [Fact]
        public void Get_ReceivesInvalidId_ReturnsNotFound()
        {
            // Arrange
            var controller = CreateController();

            // Act
            var actionResult = controller.Add(0);

            // Assert
            actionResult.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void Get_ReceivesValidId_ReturnsViewResult()
        {
            // Arrange
            var controller = CreateController();

            // Act
            var actionResult = controller.Add(1);

            // Assert
            actionResult.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public void Get_ReceivesValidId_ReturnsViewWithCorrectIdInModel()
        {
            // Arrange
            var controller = CreateController();

            // Act
            var actionResult = controller.Add(1);

            // Assert
            var resultObject = actionResult as ViewResult;
            var model = resultObject.Model;
            model.Should().BeOfType<TelephoneNumber>();
            ((TelephoneNumber) model).ContactId.Should().Be(1);
        }

        [Fact]
        public void Post_ReceivesNull_ReturnsBadRequest()
        {
            // Arrange
            var controller = CreateController();

            // Act
            var actionResult = controller.Add(null);

            // Assert
            actionResult.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public void Post_ReceivesCorrectModel_ReturnsRedirectResult()
        {
            // Arrange
            var controller = CreateController();

            // Act
            var actionResult = controller.Add(new TelephoneNumber());

            // Assert
            actionResult.Should().BeOfType<RedirectToActionResult>();
        }

        [Fact]
        public void Post_ReceivesCorrectModel_ReturnsRedirectToDetails()
        {
            // Arrange
            var controller = CreateController();

            // Act
            var actionResult = controller.Add(new TelephoneNumber());

            // Assert
            var redirect = actionResult as RedirectToActionResult;
            redirect.ActionName.Should().Be("Details");
            redirect.ControllerName.Should().Be("Dashboard");
        }
    }
}