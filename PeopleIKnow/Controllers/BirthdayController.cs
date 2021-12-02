using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeopleIKnow.DataAccess.Repositories;

namespace PeopleIKnow.Controllers
{
    [Authorize(Roles = "user")]
    public class BirthdayController : Controller
    {
        private readonly IContactRepository _contactRepository;

        public BirthdayController(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public async Task<IActionResult> Index()
        {
            var contacts = await _contactRepository.GetUpcomingBirthdaysAsync();
            return View(contacts);
        }
    }
}