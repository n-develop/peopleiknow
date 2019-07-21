using Microsoft.AspNetCore.Hosting;
using NSubstitute;
using PeopleIKnow.Controllers;
using PeopleIKnow.Repositories;

namespace PeopleIKnow.UnitTests.ControllerTests.DashboardControllerTests
{
    public abstract class BaseDashboardControllerTests
    {
        #region test infrastructure

        protected IContactRepository _contactRepository;
        private IHostingEnvironment _hostingEnvironment;

        protected BaseDashboardControllerTests()
        {
            _contactRepository = Substitute.For<IContactRepository>();
            _hostingEnvironment = Substitute.For<IHostingEnvironment>();
        }

        protected DashboardController CreateController()
        {
            return new DashboardController(_contactRepository, _hostingEnvironment);
        }

        #endregion
    }
}