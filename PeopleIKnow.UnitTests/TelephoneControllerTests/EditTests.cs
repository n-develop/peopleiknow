using System;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using PeopleIKnow.Models;
using Xunit;

namespace PeopleIKnow.UnitTests.TelephoneControllerTests
{
    public class EditTests : BaseTelephoneControllerTests
    {
        [Fact]
        public void ReceivesInvalidId_ReturnsNotFound()
        {
            // Arrange
            var controller = CreateController();

            // Act
            var actionResult = controller.Edit(0);

            // Assert
            actionResult.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void CannotFindContactById_ReturnsNotFound()
        {
            // Arrange
            _contactRepository.GetTelephoneNumberById(2).Returns(NullTelephoneNumber.GetInstance());
            var controller = CreateController();

            // Act
            var actionResult = controller.Edit(2);

            // Assert
            actionResult.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void ReceivesValidId_ReturnsView()
        {
            // Arrange
            _contactRepository.GetTelephoneNumberById(2).Returns(new TelephoneNumber {Id = 2});
            var controller = CreateController();

            // Act
            var actionResult = controller.Edit(2);

            // Assert
            actionResult.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public void ReceivesValidId_ReturnsModelWithId()
        {
            // Arrange
            _contactRepository.GetTelephoneNumberById(2).Returns(new TelephoneNumber {Id = 2});
            var controller = CreateController();

            // Act
            var actionResult = controller.Edit(2);

            // Assert
            var model = (actionResult as ViewResult).Model;
            model.Should().BeOfType<TelephoneNumber>();
            (model as TelephoneNumber).Id.Should().Be(2);
        }
    }
}