using NSubstitute;
using PeopleIKnow.Controllers;
using PeopleIKnow.DataAccess.Repositories;
using PeopleIKnow.Services;

namespace PeopleIKnow.UnitTests.ControllerTests.DashboardControllerTests
{
    public abstract class BaseDashboardControllerTests
    {
        #region test infrastructure

        protected IContactRepository _contactRepository;
        protected IImageRepository _imageRepository;

        protected BaseDashboardControllerTests()
        {
            _contactRepository = Substitute.For<IContactRepository>();
            _imageRepository = Substitute.For<IImageRepository>();
        }

        protected DashboardController CreateController()
        {
            return new DashboardController(_contactRepository, _imageRepository);
        }

        #endregion
    }
}