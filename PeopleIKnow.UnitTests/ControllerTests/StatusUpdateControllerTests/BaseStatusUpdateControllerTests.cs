using Microsoft.Extensions.Logging;
using NSubstitute;
using PeopleIKnow.Controllers;
using PeopleIKnow.Models;
using PeopleIKnow.Repositories;

namespace PeopleIKnow.UnitTests.ControllerTests.StatusUpdateControllerTests
{
    public abstract class BaseStatusUpdateControllerTests
    {
        #region test infrastructure

        protected IRepository<StatusUpdate> _repository;
        private ILogger<StatusUpdateController> _logger;

        protected BaseStatusUpdateControllerTests()
        {
            _repository = Substitute.For<IRepository<StatusUpdate>>();
            _logger = Substitute.For<ILogger<StatusUpdateController>>();
        }

        protected StatusUpdateController CreateController()
        {
            return new StatusUpdateController(_repository, _logger);
        }

        #endregion
    }
}