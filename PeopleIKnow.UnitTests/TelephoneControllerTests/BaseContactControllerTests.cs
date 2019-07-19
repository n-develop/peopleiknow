using Microsoft.Extensions.Logging;
using NSubstitute;
using PeopleIKnow.Controllers;
using PeopleIKnow.Repositories;

namespace PeopleIKnow.UnitTests.TelephoneControllerTests
{
    public abstract class BaseTelephoneControllerTests
    {
        #region test infrastructure

        protected IContactRepository _contactRepository;
        private ILogger<TelephoneController> _logger;

        protected BaseTelephoneControllerTests()
        {
            _contactRepository = Substitute.For<IContactRepository>();
            _logger = Substitute.For<ILogger<TelephoneController>>();
        }

        protected TelephoneController CreateController()
        {
            return new TelephoneController(_contactRepository, _logger);
        }

        #endregion
    }
}