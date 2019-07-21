using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace PeopleIKnow.UnitTests.ControllerTests.DashboardControllerTests
{
    public class IndexTests : BaseDashboardControllerTests
    {
        [Fact]
        public void ReturnsView()
        {
            // Arrange
            var controller = CreateController();

            // Act
            var actionResult = controller.Index();

            // Assert
            actionResult.Should().BeOfType<ViewResult>();
        }
    }
}