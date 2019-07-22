using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using PeopleIKnow.Models;
using Xunit;

namespace PeopleIKnow.UnitTests.ControllerTests.TelephoneControllerTests
{
    public class DeleteTests : BaseTelephoneControllerTests
    {
        [Fact]
        public void ReceivesInvalidId_ReturnsNotFound()
        {
            // Arrange
            var controller = CreateController();

            // Act
            var actionResult = controller.Delete(0);

            // Assert
            actionResult.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void CannotFindTelephoneNumber_ReturnsNotFound()
        {
            // Arrange
            _contactRepository.GetTelephoneNumberById(1).Returns(NullTelephoneNumber.GetInstance());
            var controller = CreateController();

            // Act
            var actionResult = controller.Delete(1);

            // Assert
            actionResult.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void ReceivesValidId_ReturnRedirect()
        {
            // Arrange
            _contactRepository.GetTelephoneNumberById(1).Returns(new TelephoneNumber {ContactId = 2});
            var controller = CreateController();

            // Act
            var actionResult = controller.Delete(1);

            // Assert
            actionResult.Should().BeOfType<RedirectToActionResult>();
        }

        [Fact]
        public void ReceivesValidId_ReturnsRedirectToDetails()
        {
            // Arrange
            _contactRepository.GetTelephoneNumberById(1).Returns(new TelephoneNumber {ContactId = 2});
            var controller = CreateController();

            // Act
            var redirectResult = controller.Delete(1) as RedirectToActionResult;

            // Assert
            redirectResult.ActionName.Should().Be("Details");
            redirectResult.ControllerName.Should().Be("Dashboard");
        }
    }
}