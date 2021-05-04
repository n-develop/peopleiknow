using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using PeopleIKnow.Models;
using Xunit;

namespace PeopleIKnow.UnitTests.ControllerTests.RelationshipControllerTests
{
    public class EditTests : BaseRelationshipControllerTests
    {
        [Fact]
        public async Task Get_ReceivesInvalidId_ReturnsNotFound()
        {
            // Arrange
            var controller = CreateController();

            // Act
            var actionResult = await controller.Edit(0);

            // Assert
            actionResult.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Get_CannotFindContactById_ReturnsNotFound()
        {
            // Arrange
            _repository.GetByIdAsync(2).ReturnsNull();
            var controller = CreateController();

            // Act
            var actionResult = await controller.Edit(2);

            // Assert
            actionResult.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Get_ReceivesValidId_ReturnsView()
        {
            // Arrange
            _repository.GetByIdAsync(2).Returns(new Relationship {Id = 2});
            var controller = CreateController();

            // Act
            var actionResult = await controller.Edit(2);

            // Assert
            actionResult.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public async Task Get_ReceivesValidId_ReturnsModelWithId()
        {
            // Arrange
            _repository.GetByIdAsync(2).Returns(new Relationship {Id = 2});
            var controller = CreateController();

            // Act
            var actionResult = await controller.Edit(2);

            // Assert
            var model = (actionResult as ViewResult).Model;
            model.Should().BeOfType<Relationship>();
            (model as Relationship).Id.Should().Be(2);
        }

        [Fact]
        public async Task Post_ReceivesNullAsModel_ReturnsBadRequest()
        {
            // Arrange
            var controller = CreateController();

            // Act
            var actionResult = await controller.Edit(null);

            // Assert
            actionResult.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task Post_ReceivesValidMode_ReturnsRedirect()
        {
            // Arrange
            var controller = CreateController();

            // Act
            var actionResult = await controller.Edit(new Relationship());

            // Assert
            actionResult.Should().BeOfType<RedirectToActionResult>();
        }

        [Fact]
        public async Task Post_ReceivesValidModel_ReturnsRedirectToDetails()
        {
            // Arrange
            var controller = CreateController();

            // Act
            var actionResult = await controller.Edit(new Relationship()) as RedirectToActionResult;

            // Assert
            actionResult.ActionName.Should().Be("Details");
            actionResult.ControllerName.Should().Be("Dashboard");
        }
    }
}