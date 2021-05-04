using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using PeopleIKnow.Models;
using Xunit;

namespace PeopleIKnow.UnitTests.ControllerTests.StatusUpdateControllerTests
{
    public class DeleteTests : BaseStatusUpdateControllerTests
    {
        [Fact]
        public async Task ReceivesInvalidId_ReturnsNotFound()
        {
            // Arrange
            var controller = CreateController();

            // Act
            var actionResult = await controller.Delete(0);

            // Assert
            actionResult.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task CannotFindStatusUpdate_ReturnsNotFound()
        {
            // Arrange
            _repository.GetByIdAsync(1).ReturnsNull();
            var controller = CreateController();

            // Act
            var actionResult = await controller.Delete(1);

            // Assert
            actionResult.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task ReceivesValidId_ReturnRedirect()
        {
            // Arrange
            _repository.GetByIdAsync(1).Returns(new StatusUpdate {ContactId = 2});
            var controller = CreateController();

            // Act
            var actionResult = await controller.Delete(1);

            // Assert
            actionResult.Should().BeOfType<RedirectToActionResult>();
        }

        [Fact]
        public async Task ReceivesValidId_ReturnsRedirectToDetails()
        {
            // Arrange
            _repository.GetByIdAsync(1).Returns(new StatusUpdate {ContactId = 2});
            var controller = CreateController();

            // Act
            var redirectResult = await controller.Delete(1) as RedirectToActionResult;

            // Assert
            redirectResult.ActionName.Should().Be("Details");
            redirectResult.ControllerName.Should().Be("Dashboard");
        }
    }
}