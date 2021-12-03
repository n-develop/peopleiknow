using NSubstitute;
using PeopleIKnow.Controllers;
using PeopleIKnow.DataAccess.Repositories;
using PeopleIKnow.Services;

namespace PeopleIKnow.UnitTests.ControllerTests.DashboardControllerTests
{
    public abstract class BaseDashboardControllerTests
    {
        #region test infrastructure

        protected DashboardController CreateController()
        {
            return new DashboardController();
        }

        #endregion
    }
}