using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using PeopleIKnow.Controllers;
using PeopleIKnow.Models;
using PeopleIKnow.Repositories;
using Xunit;

namespace PeopleIKnow.UnitTests.ContactControllerTests
{
    public class TeaserTests : BaseContactControllerTests
    {
        [Fact]
        public void ReceivesZeroId_ReturnsNotFound()
        {
            // Arrange
            var controller = CreateController();

            // Act
            var actionResult = controller.Teaser(0);

            // Assert
            actionResult.Should().NotBeNull();
            actionResult.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void CannotFindContact_ReturnsNotFound()
        {
            // Arrange
            _contactRepository.GetContactById(1).Returns(NullContact.GetInstance());
            var controller = CreateController();

            // Act
            var actionResult = controller.Teaser(1);

            // Assert
            actionResult.Should().NotBeNull();
            actionResult.Should().BeOfType<NotFoundResult>();
        }

        [Fact(Skip = "Still one reference missing")]
        public void FindsContact_ReturnsView()
        {
            // Arrange
            _contactRepository.GetContactById(2).Returns(new Contact
            {
                Firstname = "not important",
                Middlename = "not important",
                Lastname = "not important",
                Address = "not important",
            });
            var controller = CreateController();

            // Act
            var actionResult = controller.Teaser(2);

            // Assert
            actionResult.Should().NotBeNull();
            actionResult.Should().BeOfType<ViewResult>();
        }
    }
}