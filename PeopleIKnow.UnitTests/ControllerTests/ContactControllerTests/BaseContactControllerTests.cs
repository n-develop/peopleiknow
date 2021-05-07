using Microsoft.Extensions.Logging;
using NSubstitute;
using PeopleIKnow.Controllers;
using PeopleIKnow.DataAccess.Repositories;

namespace PeopleIKnow.UnitTests.ControllerTests.ContactControllerTests
{
    public abstract class BaseContactControllerTests
    {
        #region test infrastructure

        protected IContactRepository _contactRepository;
        private ILogger<ContactController> _logger;

        protected BaseContactControllerTests()
        {
            _contactRepository = Substitute.For<IContactRepository>();
            _logger = Substitute.For<ILogger<ContactController>>();
        }

        protected ContactController CreateController()
        {
            return new ContactController(_contactRepository, _logger);
        }

        #endregion
    }
}