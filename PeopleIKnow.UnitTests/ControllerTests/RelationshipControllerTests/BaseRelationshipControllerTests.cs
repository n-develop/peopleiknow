using Microsoft.Extensions.Logging;
using NSubstitute;
using PeopleIKnow.Controllers;
using PeopleIKnow.Models;
using PeopleIKnow.Repositories;

namespace PeopleIKnow.UnitTests.ControllerTests.RelationshipControllerTests
{
    public abstract class BaseRelationshipControllerTests
    {
        #region test infrastructure

        protected IRepository<Relationship> _repository;
        private ILogger<RelationshipController> _logger;

        protected BaseRelationshipControllerTests()
        {
            _repository = Substitute.For<IRepository<Relationship>>();
            _logger = Substitute.For<ILogger<RelationshipController>>();
        }

        protected RelationshipController CreateController()
        {
            return new RelationshipController(_repository, _logger);
        }

        #endregion
    }
}