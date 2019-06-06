using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PeopleIKnow.Repositories;

namespace PeopleIKnow.Controllers
{
    public class SearchController : Controller
    {
        private readonly IContactRepository _repository;

        public SearchController(IContactRepository repository)
        {
            _repository = repository;
        }

        public async Task<IActionResult> Index(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                return RedirectToAction("ContactList", "Dashboard");
            }

            var searchResult = await _repository.SearchContacts(term);

            return View("Components/ContactList/Default", searchResult.OrderBy(p => p.Lastname).ToList());
        }
    }
}