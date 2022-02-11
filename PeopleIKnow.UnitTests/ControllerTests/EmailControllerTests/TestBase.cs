using Microsoft.Extensions.Logging;
using NSubstitute;
using PeopleIKnow.Controllers;
using PeopleIKnow.DataAccess.Repositories;
using PeopleIKnow.Models;

namespace PeopleIKnow.UnitTests.ControllerTests.EmailControllerTests
{
    public abstract class TestBase
    {
        #region test infrastructure

        protected IRepository<EmailAddress> _repository;
        private ILogger<EmailController> _logger;
        protected readonly EmailController _sut;
        protected IContactRepository _contactRepository;

        protected TestBase()
        {
            _repository = Substitute.For<IRepository<EmailAddress>>();
            _logger = Substitute.For<ILogger<EmailController>>();
            _contactRepository = Substitute.For<IContactRepository>();
            _sut = new EmailController(_repository, _contactRepository, _logger);
        }

        #endregion
    }
}