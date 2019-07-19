using Microsoft.Extensions.Logging;
using NSubstitute;
using PeopleIKnow.Controllers;
using PeopleIKnow.Repositories;

namespace PeopleIKnow.UnitTests.ControllerTests.RelationshipControllerTests
{
    public abstract class BaseRelationshipControllerTests
    {
        #region test infrastructure

        protected IContactRepository _contactRepository;
        private ILogger<RelationshipController> _logger;

        protected BaseRelationshipControllerTests()
        {
            _contactRepository = Substitute.For<IContactRepository>();
            _logger = Substitute.For<ILogger<RelationshipController>>();
        }

        protected RelationshipController CreateController()
        {
            return new RelationshipController(_contactRepository, _logger);
        }

        #endregion
    }
}