using Microsoft.Extensions.Logging;
using NSubstitute;
using PeopleIKnow.Controllers;
using PeopleIKnow.Repositories;

namespace PeopleIKnow.UnitTests.ControllerTests.StatusUpdateControllerTests
{
    public abstract class BaseStatusUpdateControllerTests
    {
        #region test infrastructure

        protected IContactRepository _contactRepository;
        private ILogger<StatusUpdateController> _logger;

        protected BaseStatusUpdateControllerTests()
        {
            _contactRepository = Substitute.For<IContactRepository>();
            _logger = Substitute.For<ILogger<StatusUpdateController>>();
        }

        protected StatusUpdateController CreateController()
        {
            return new StatusUpdateController(_contactRepository, _logger);
        }

        #endregion
    }
}