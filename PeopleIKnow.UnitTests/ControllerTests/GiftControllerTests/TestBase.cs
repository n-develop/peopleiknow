using Microsoft.Extensions.Logging;
using NSubstitute;
using PeopleIKnow.Controllers;
using PeopleIKnow.DataAccess.Repositories;
using PeopleIKnow.Models;

namespace PeopleIKnow.UnitTests.ControllerTests.GiftControllerTests
{
    public class TestBase
    {
        #region test infrastructure

        protected GiftController _sut;
        protected IRepository<Gift> _repository;
        private ILogger<GiftController> _logger;
        protected IContactRepository _contactRepository;

        public TestBase()
        {
            _repository = Substitute.For<IRepository<Gift>>();
            _logger = Substitute.For<ILogger<GiftController>>();
            _contactRepository = Substitute.For<IContactRepository>();
            _sut = new GiftController(_logger, _contactRepository, _repository);
        }

        #endregion
    }
}