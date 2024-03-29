using System;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using PeopleIKnow.Controllers;
using PeopleIKnow.DataAccess.Repositories;
using Xunit;

namespace PeopleIKnow.UnitTests.ControllerTests.ContactControllerTests
{
    public class DeleteTests : TestBase
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
            _contactRepository.When(fake => fake.DeleteContact(1)).Throw<ContactNotFoundException>();
            var controller = CreateController();

            // Act
            var actionResult = controller.Delete(1);

            // Assert
            actionResult.Should().NotBeNull();
            actionResult.Should().BeOfType<JsonResult>();
        }

        [Fact]
        public void Successful_ReturnsSuccessTrue()
        {
            // Arrange
            var controller = CreateController();

            // Act
            var actionResult = controller.Delete(1);

            // Assert
            actionResult.Should().NotBeNull();
            var resultObject = ((JsonResult) actionResult).Value;
            var success = GetPropertyByName(resultObject, "Success");
            success.Should().Be(true);
        }

        [Fact]
        public void Successful_ReturnsSuccessfulMessage()
        {
            // Arrange
            var controller = CreateController();

            // Act
            var actionResult = controller.Delete(1);

            // Assert
            actionResult.Should().NotBeNull();
            var resultObject = ((JsonResult) actionResult).Value;
            var message = GetPropertyByName(resultObject, "Message");
            message.Should().Be(ContactController.SuccessfullyDeletedMessage);
        }

        [Fact]
        public void Unsuccessful_ReturnsSuccessFalse()
        {
            // Arrange
            _contactRepository.When(fake => fake.DeleteContact(1)).Throw<ContactNotFoundException>();
            var controller = CreateController();

            // Act
            var actionResult = controller.Delete(1);

            // Assert
            actionResult.Should().NotBeNull();
            var resultObject = ((JsonResult) actionResult).Value;
            var success = GetPropertyByName(resultObject, "Success");
            success.Should().Be(false);
        }

        [Fact]
        public void Unsuccessful_ReturnsUnsuccessfulMessage()
        {
            // Arrange
            _contactRepository.When(fake => fake.DeleteContact(1)).Throw(new Exception("ERROR"));
            var controller = CreateController();

            // Act
            var actionResult = controller.Delete(1);

            // Assert
            actionResult.Should().NotBeNull();
            var resultObject = ((JsonResult) actionResult).Value;
            var message = GetPropertyByName(resultObject, "Message");
            message.Should().Be(ContactController.ContactCannotBeDeleted + " ERROR");
        }

        private object GetPropertyByName(object obj, string propertyName)
        {
            var property = obj.GetType().GetProperty(propertyName);
            var result = property.GetValue(obj);
            return result;
        }
    }
}