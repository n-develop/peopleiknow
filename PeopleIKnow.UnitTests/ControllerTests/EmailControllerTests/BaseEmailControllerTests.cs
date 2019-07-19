using Microsoft.Extensions.Logging;
using NSubstitute;
using PeopleIKnow.Controllers;
using PeopleIKnow.Repositories;

namespace PeopleIKnow.UnitTests.ControllerTests.EmailControllerTests
{
    public abstract class BaseEmailControllerTests
    {
        #region test infrastructure

        protected IContactRepository _contactRepository;
        private ILogger<EmailController> _logger;

        protected BaseEmailControllerTests()
        {
            _contactRepository = Substitute.For<IContactRepository>();
            _logger = Substitute.For<ILogger<EmailController>>();
        }

        protected EmailController CreateController()
        {
            return new EmailController(_contactRepository, _logger);
        }

        #endregion
    }
}