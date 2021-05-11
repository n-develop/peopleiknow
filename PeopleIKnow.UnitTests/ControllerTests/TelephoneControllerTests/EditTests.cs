using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using PeopleIKnow.Models;
using Xunit;

namespace PeopleIKnow.UnitTests.ControllerTests.TelephoneControllerTests
{
    public class EditTests : TestBase
    {
        [Fact]
        public async Task Get_ReceivesInvalidId_ReturnsNotFound()
        {
            // Arrange
            // Act
            var actionResult = await _sut.Edit(0);

            // Assert
            actionResult.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Get_CannotFindContactById_ReturnsNotFound()
        {
            // Arrange
            _repository.GetByIdAsync(2).ReturnsNull();

            // Act
            var actionResult = await _sut.Edit(2);

            // Assert
            actionResult.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Get_ReceivesValidId_ReturnsView()
        {
            // Arrange
            _repository.GetByIdAsync(2).Returns(new TelephoneNumber {Id = 2});

            // Act
            var actionResult = await _sut.Edit(2);

            // Assert
            actionResult.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public async Task Get_ReceivesValidId_ReturnsModelWithId()
        {
            // Arrange
            _repository.GetByIdAsync(2).Returns(new TelephoneNumber {Id = 2});

            // Act
            var actionResult = await _sut.Edit(2);

            // Assert
            var model = (actionResult as ViewResult).Model;
            model.Should().BeOfType<TelephoneNumber>();
            (model as TelephoneNumber).Id.Should().Be(2);
        }

        [Fact]
        public async Task Post_ReceivesNullAsModel_ReturnsBadRequest()
        {
            // Arrange
            // Act
            var actionResult = await _sut.Edit(null);

            // Assert
            actionResult.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task Post_ReceivesValidMode_ReturnsRedirect()
        {
            // Arrange
            // Act
            var actionResult = await _sut.Edit(new TelephoneNumber());

            // Assert
            actionResult.Should().BeOfType<RedirectToActionResult>();
        }

        [Fact]
        public async Task Post_ReceivesValidModel_ReturnsRedirectToDetails()
        {
            // Arrange
            // Act
            var actionResult = await _sut.Edit(new TelephoneNumber()) as RedirectToActionResult;

            // Assert
            actionResult.ActionName.Should().Be("Details");
            actionResult.ControllerName.Should().Be("Dashboard");
        }
    }
}