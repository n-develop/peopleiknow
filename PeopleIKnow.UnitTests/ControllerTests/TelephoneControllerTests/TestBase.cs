using Microsoft.Extensions.Logging;
using NSubstitute;
using PeopleIKnow.Controllers;
using PeopleIKnow.DataAccess.Repositories;
using PeopleIKnow.Models;

namespace PeopleIKnow.UnitTests.ControllerTests.TelephoneControllerTests
{
    public abstract class TestBase
    {
        #region test infrastructure

        protected IRepository<TelephoneNumber> _repository;
        private ILogger<TelephoneController> _logger;
        protected TelephoneController _sut;
        protected IContactRepository _contactRepository;

        protected TestBase()
        {
            _repository = Substitute.For<IRepository<TelephoneNumber>>();
            _logger = Substitute.For<ILogger<TelephoneController>>();
            _contactRepository = Substitute.For<IContactRepository>();
            _sut = new TelephoneController(_repository, _contactRepository, _logger);
        }

        #endregion
    }
}