using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeopleIKnow.DataAccess.Repositories;

namespace PeopleIKnow.Controllers
{
    [Authorize(Roles = "user")]
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
            var orderedContacts = searchResult
                .OrderByDescending(p => p.IsFavorite)
                .ThenBy(p => p.Lastname)
                .ToList();

            return View("Components/ContactList/Default", orderedContacts);
        }
    }
}