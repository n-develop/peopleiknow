using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using PeopleIKnow.Models;
using Xunit;

namespace PeopleIKnow.UnitTests.ControllerTests.ContactControllerTests
{
    public class FavoriteTests : BaseContactControllerTests
    {
        [Fact]
        public void InvalidId_ReturnsNotFound()
        {
            // Arrange
            var controller = CreateController();

            // Act
            var actionResult = controller.Favorite(0);

            // Assert
            actionResult.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void UnknownId_ReturnsNotFound()
        {
            // Arrange
            _contactRepository.GetContactById(1).Returns(NullContact.GetInstance());
            var controller = CreateController();

            // Act
            var actionResult = controller.Favorite(1);

            // Assert
            actionResult.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void ChangedStateSuccessfully_ReturnsViewComponentContactList()
        {
            // Arrange
            _contactRepository.GetContactById(2).Returns(new Contact {Id = 2, IsFavorite = false});
            var controller = CreateController();

            // Act
            var actionResult = controller.Favorite(2);

            // Assert
            actionResult.Should().BeOfType<ViewComponentResult>();
        }

        [Fact]
        public void FavoriteIsInitiallyFalse_FavoriteTurnsTrue()
        {
            // Arrange
            var contact = new Contact {Id = 3, IsFavorite = false};
            _contactRepository.GetContactById(3).Returns(contact);
            var controller = CreateController();

            // Act
            controller.Favorite(3);

            // Assert
            contact.IsFavorite.Should().BeTrue();
        }

        [Fact]
        public void FavoriteIsInitiallyTrue_FavoriteTurnsFalse()
        {
            // Arrange
            var contact = new Contact {Id = 3, IsFavorite = true};
            _contactRepository.GetContactById(3).Returns(contact);
            var controller = CreateController();

            // Act
            controller.Favorite(3);

            // Assert
            contact.IsFavorite.Should().BeFalse();
        }
    }
}