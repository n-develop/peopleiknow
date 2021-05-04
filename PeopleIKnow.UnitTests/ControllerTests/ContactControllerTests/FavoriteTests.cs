using System.Threading.Tasks;
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
        public async Task InvalidId_ReturnsNotFound()
        {
            // Arrange
            var controller = CreateController();

            // Act
            var actionResult = await controller.Favorite(0);

            // Assert
            actionResult.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task UnknownId_ReturnsNotFound()
        {
            // Arrange
            _contactRepository.GetContactById(1).Returns(NullContact.GetInstance());
            var controller = CreateController();

            // Act
            var actionResult = await controller.Favorite(1);

            // Assert
            actionResult.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task ChangedStateSuccessfully_ReturnsViewComponentContactList()
        {
            // Arrange
            _contactRepository.GetContactById(2).Returns(new Contact {Id = 2, IsFavorite = false});
            var controller = CreateController();

            // Act
            var actionResult = await controller.Favorite(2);

            // Assert
            actionResult.Should().BeOfType<ViewComponentResult>();
        }

        [Fact]
        public async Task FavoriteIsInitiallyFalse_FavoriteTurnsTrue()
        {
            // Arrange
            var contact = new Contact {Id = 3, IsFavorite = false};
            _contactRepository.GetContactById(3).Returns(contact);
            var controller = CreateController();

            // Act
            await controller.Favorite(3);

            // Assert
            contact.IsFavorite.Should().BeTrue();
        }

        [Fact]
        public async Task FavoriteIsInitiallyTrue_FavoriteTurnsFalse()
        {
            // Arrange
            var contact = new Contact {Id = 3, IsFavorite = true};
            _contactRepository.GetContactById(3).Returns(contact);
            var controller = CreateController();

            // Act
            await controller.Favorite(3);

            // Assert
            contact.IsFavorite.Should().BeFalse();
        }
    }
}