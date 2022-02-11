using Microsoft.Extensions.Logging;
using NSubstitute;
using PeopleIKnow.Controllers;
using PeopleIKnow.DataAccess.Repositories;
using PeopleIKnow.Models;

namespace PeopleIKnow.UnitTests.ControllerTests.CommonActivityControllerTests
{
    public class TestBase
    {
        #region test infrastructure

        protected readonly CommonActivityController _sut;
        private ILogger<CommonActivityController> _logger;
        protected IRepository<CommonActivity> _repository;
        protected IContactRepository _contactRepository;

        public TestBase()
        {
            _logger = Substitute.For<ILogger<CommonActivityController>>();
            _repository = Substitute.For<IRepository<CommonActivity>>();
            _contactRepository = Substitute.For<IContactRepository>();
            _sut = new CommonActivityController(_repository, _contactRepository, _logger);
        }

        #endregion
    }
}