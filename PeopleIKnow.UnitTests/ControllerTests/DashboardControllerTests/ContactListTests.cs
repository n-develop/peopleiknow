using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace PeopleIKnow.UnitTests.ControllerTests.DashboardControllerTests
{
    public class ContactListTests : BaseDashboardControllerTests
    {
        [Fact]
        public void ReturnsViewComponent()
        {
            // Arrange
            var controller = CreateController();

            // Act
            var actionResult = controller.ContactList();

            // Assert
            actionResult.Should().BeOfType<ViewComponentResult>();
        }
    }
}