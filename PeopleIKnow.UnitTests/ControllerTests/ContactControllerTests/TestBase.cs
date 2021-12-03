using Microsoft.Extensions.Logging;
using NSubstitute;
using PeopleIKnow.Controllers;
using PeopleIKnow.DataAccess.Repositories;

namespace PeopleIKnow.UnitTests.ControllerTests.ContactControllerTests
{
    public abstract class TestBase
    {
        #region test infrastructure

        protected IContactRepository _contactRepository;
        protected IImageRepository _imageRepository;
        private ILogger<ContactController> _logger;

        protected TestBase()
        {
            _contactRepository = Substitute.For<IContactRepository>();
            _imageRepository = Substitute.For<IImageRepository>();
            _logger = Substitute.For<ILogger<ContactController>>();
        }

        protected ContactController CreateController()
        {
            return new ContactController(_contactRepository, _logger, _imageRepository);
        }

        #endregion
    }
}