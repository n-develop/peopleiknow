using Microsoft.Extensions.Logging;
using NSubstitute;
using PeopleIKnow.Controllers;
using PeopleIKnow.DataAccess.Repositories;
using PeopleIKnow.Models;

namespace PeopleIKnow.UnitTests.ControllerTests.StatusUpdateControllerTests
{
    public abstract class TestBase
    {
        #region test infrastructure

        protected IRepository<StatusUpdate> _repository;
        private ILogger<StatusUpdateController> _logger;
        protected readonly StatusUpdateController _sut;

        protected TestBase()
        {
            _repository = Substitute.For<IRepository<StatusUpdate>>();
            _logger = Substitute.For<ILogger<StatusUpdateController>>();
            _sut = new StatusUpdateController(_repository, _logger);
        }

        #endregion
    }
}