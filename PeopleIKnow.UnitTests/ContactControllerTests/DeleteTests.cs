using System.Reflection;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;

namespace PeopleIKnow.UnitTests.ContactControllerTests
{
    public class DeleteTests : BaseContactControllerTests
    {
        [Fact]
        public void ReceivesZeroId_ReturnsNotFound()
        {
            // Arrange
            var controller = CreateController();

            // Act
            var actionResult = controller.Delete(0);

            // Assert
            actionResult.Should().NotBeNull();
            actionResult.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void NotSuccessful_ReturnsJsonResult()
        {
            // Arrange
            _contactRepository.DeleteContact(1).Returns(false);
            var controller = CreateController();

            // Act
            var actionResult = controller.Delete(1);

            // Assert
            actionResult.Should().NotBeNull();
            actionResult.Should().BeOfType<JsonResult>();
        }
    }
}