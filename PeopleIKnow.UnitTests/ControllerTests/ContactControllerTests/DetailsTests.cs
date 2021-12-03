using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using PeopleIKnow.Models;
using PeopleIKnow.ViewModels;
using Xunit;

namespace PeopleIKnow.UnitTests.ControllerTests.ContactControllerTests
{
    public class DetailsTests : TestBase
    {
        [Fact]
        public void Get_ReceivesInvalidId_ReturnsNotFound()
        {
            // Arrange
            var controller = CreateController();

            // Act
            var actionResult = controller.Details(0);

            // Assert
            actionResult.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void Get_CannotFindContact_ReturnsNotFound()
        {
            // Arrange
            _contactRepository.GetContactById(1).Returns(NullContact.GetInstance());
            var controller = CreateController();

            // Act
            var actionResult = controller.Details(1);

            // Assert
            actionResult.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public void Get_FoundContact_ReturnsView()
        {
            // Arrange
            _contactRepository.GetContactById(2).Returns(new Contact());
            var controller = CreateController();

            // Act
            var actionResult = controller.Details(2);

            // Assert
            actionResult.Should().BeOfType<ViewResult>();
        }

        [Fact]
        public void Get_FoundContact_ReturnsContactAsModel()
        {
            // Arrange
            _contactRepository.GetContactById(3).Returns(new Contact { Id = 3 });
            var controller = CreateController();

            // Act
            var viewResult = controller.Details(3) as ViewResult;

            // Assert
            viewResult.Model.Should().BeOfType<Contact>();
            (viewResult.Model as Contact).Id.Should().Be(3);
        }

        [Fact]
        public async Task Post_ReceivesNull_ReturnsBadRequest()
        {
            // Arrange
            var controller = CreateController();

            // Act
            var actionResult = await controller.Details(null);

            // Assert
            actionResult.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task Post_ReceivesUnknownId_ReturnsNotFound()
        {
            // Arrange
            _contactRepository.GetContactById(1).Returns(NullContact.GetInstance());
            var controller = CreateController();

            // Act
            var actionResult = await controller.Details(new ContactUpdateViewModel { Id = 1 });

            // Assert
            actionResult.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Post_ReceivesValidModel_SavesPropertiesCorrectly()
        {
            // Arrange
            var contact = new Contact { Id = 1 };
            _contactRepository.GetContactById(1).Returns(contact);
            _imageRepository.WriteFileToDiskAsync(Arg.Any<IFormFile>(), 1).Returns("test.jpg");
            var controller = CreateController();
            var imageFile = Substitute.For<IFormFile>();
            imageFile.Length.Returns(1);

            // Act
            var actionResult = await controller.Details(new ContactUpdateViewModel
            {
                Id = 1,
                Address = "Address",
                Birthday = new DateTime(2001, 1, 1),
                BusinessTitle = "BusinessTitle",
                Employer = "Employer",
                Firstname = "Firstname",
                Middlename = "Middlename",
                Lastname = "Lastname",
                Tags = "Tags",
                Image = imageFile,
            });

            // Assert
            contact.Address.Should().Be("Address");
            contact.Birthday.Should().Be(new DateTime(2001, 1, 1));
            contact.BusinessTitle.Should().Be("BusinessTitle");
            contact.Employer.Should().Be("Employer");
            contact.Firstname.Should().Be("Firstname");
            contact.Middlename.Should().Be("Middlename");
            contact.Lastname.Should().Be("Lastname");
            contact.Tags.Should().Be("Tags");
            contact.ImagePath.Should().Be("test.jpg");
        }
    }
}