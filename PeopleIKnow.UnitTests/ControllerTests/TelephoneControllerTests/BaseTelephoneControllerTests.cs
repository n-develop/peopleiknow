using Microsoft.Extensions.Logging;
using NSubstitute;
using PeopleIKnow.Controllers;
using PeopleIKnow.Models;
using PeopleIKnow.Repositories;

namespace PeopleIKnow.UnitTests.ControllerTests.TelephoneControllerTests
{
    public abstract class BaseTelephoneControllerTests
    {
        #region test infrastructure

        protected IRepository<TelephoneNumber> _repository;
        private ILogger<TelephoneController> _logger;

        protected BaseTelephoneControllerTests()
        {
            _repository = Substitute.For<IRepository<TelephoneNumber>>();
            _logger = Substitute.For<ILogger<TelephoneController>>();
        }

        protected TelephoneController CreateController()
        {
            return new TelephoneController(_repository, _logger);
        }

        #endregion
    }
}