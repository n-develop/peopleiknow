using Microsoft.Extensions.Logging;
using NSubstitute;
using PeopleIKnow.Controllers;
using PeopleIKnow.DataAccess.Repositories;
using PeopleIKnow.Models;

namespace PeopleIKnow.UnitTests.ControllerTests.RelationshipControllerTests
{
    public abstract class TestBase
    {
        #region test infrastructure

        protected IRepository<Relationship> _repository;
        private ILogger<RelationshipController> _logger;
        protected readonly RelationshipController _sut;

        protected TestBase()
        {
            _repository = Substitute.For<IRepository<Relationship>>();
            _logger = Substitute.For<ILogger<RelationshipController>>();
            _sut = new RelationshipController(_repository, _logger);
        }

        #endregion
    }
}