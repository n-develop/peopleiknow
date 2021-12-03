using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using PeopleIKnow.Models;
using Xunit;

namespace PeopleIKnow.UnitTests.ControllerTests.CommonActivityControllerTests
{
    public class AddTests : TestBase
    {
        [Fact]
        public void Get_ReceivesInvalidId_ReturnsNotFound()
        {
            // Arrange
            // Act
            var actionResult = _sut.Add(0);

            // Assert
            actionResult.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void Get_ReceivesValidId_ReturnsViewResult()
        {
            // Arrange
            // Act
            var actionResult = _sut.Add(1);

            // Assert
            actionResult.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public void Get_ReceivesValidId_ReturnsViewWithCorrectIdInModel()
        {
            // Arrange
            // Act
            var actionResult = _sut.Add(1);

            // Assert
            var resultObject = actionResult as ViewResult;
            var model = resultObject.Model;
            model.Should().BeOfType<CommonActivity>();
            ((CommonActivity)model).ContactId.Should().Be(1);
        }

        [Fact]
        public async Task Post_ReceivesNull_ReturnsBadRequest()
        {
            // Arrange
            // Act
            var actionResult = await _sut.Add(null);

            // Assert
            actionResult.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task Post_ReceivesCorrectModel_ReturnsRedirectResult()
        {
            // Arrange
            // Act
            var actionResult = await _sut.Add(new CommonActivity());

            // Assert
            actionResult.Should().BeOfType<RedirectToActionResult>();
        }

        [Fact]
        public async Task Post_ReceivesCorrectModel_ReturnsRedirectToDetails()
        {
            // Arrange
            // Act
            var actionResult = await _sut.Add(new CommonActivity());

            // Assert
            var redirect = actionResult as RedirectToActionResult;
            redirect.ActionName.Should().Be("Details");
            redirect.ControllerName.Should().Be("Contact");
        }
    }
}