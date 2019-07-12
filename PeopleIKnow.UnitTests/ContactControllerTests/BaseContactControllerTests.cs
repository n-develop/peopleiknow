using NSubstitute;
using PeopleIKnow.Controllers;
using PeopleIKnow.Repositories;

namespace PeopleIKnow.UnitTests.ContactControllerTests
{
    public abstract class BaseContactControllerTests
    {
        #region test infrastructure

        protected IContactRepository _contactRepository;

        protected BaseContactControllerTests()
        {
            _contactRepository = Substitute.For<IContactRepository>();
        }

        protected ContactController CreateController()
        {
            return new ContactController(_contactRepository);
        }

        #endregion
    }
}