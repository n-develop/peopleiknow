using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using PeopleIKnow.Models;
using Xunit;

namespace PeopleIKnow.UnitTests.ControllerTests.GiftControllerTests
{
    public class DeleteTests : BaseGiftControllerTests
    {
        [Fact]
        public async Task ReceivesInvalidId_ReturnsNotFound()
        {
            // Arrange
            // Act
            var actionResult = await _sut.Delete(0);

            // Assert
            actionResult.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task CannotFindGift_ReturnsNotFound()
        {
            // Arrange
            _repository.GetByIdAsync(1).ReturnsNull();

            // Act
            var actionResult = await _sut.Delete(1);

            // Assert
            actionResult.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task ReceivesValidId_ReturnRedirect()
        {
            // Arrange
            _repository.GetByIdAsync(1).Returns(new Gift {ContactId = 2});

            // Act
            var actionResult = await _sut.Delete(1);

            // Assert
            actionResult.Should().BeOfType<RedirectToActionResult>();
        }

        [Fact]
        public async Task ReceivesValidId_ReturnsRedirectToDetails()
        {
            // Arrange
            _repository.GetByIdAsync(1).Returns(new Gift {ContactId = 2});

            // Act
            var redirectResult = await _sut.Delete(1) as RedirectToActionResult;

            // Assert
            redirectResult.ActionName.Should().Be("Details");
            redirectResult.ControllerName.Should().Be("Dashboard");
        }
    }
}