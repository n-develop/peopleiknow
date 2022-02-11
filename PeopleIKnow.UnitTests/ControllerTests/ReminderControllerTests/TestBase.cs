using Microsoft.Extensions.Logging;
using NSubstitute;
using PeopleIKnow.Controllers;
using PeopleIKnow.DataAccess.Repositories;
using PeopleIKnow.Models;

namespace PeopleIKnow.UnitTests.ControllerTests.ReminderControllerTests
{
    public class BaseReminderControllerTests
    {
        #region test infrastructure

        protected ReminderController _sut;
        protected IRepository<Reminder> _repository;
        private ILogger<ReminderController> _logger;
        protected IContactRepository _contactRepository;

        public BaseReminderControllerTests()
        {
            _logger = Substitute.For<ILogger<ReminderController>>();
            _repository = Substitute.For<IRepository<Reminder>>();
            _contactRepository = Substitute.For<IContactRepository>();
            _sut = new ReminderController(_logger, _contactRepository, _repository);
        }

        #endregion
    }
}