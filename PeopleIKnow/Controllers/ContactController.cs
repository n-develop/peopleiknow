using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PeopleIKnow.Models;
using PeopleIKnow.Repositories;

namespace PeopleIKnow.Controllers
{
    [Authorize]
    public class ContactController : Controller
    {
        public static readonly string SuccessfullyDeletedMessage = "Successfully deleted";
        public static readonly string ContactCannotBeDeleted = "Contact cannot be deleted";
        private readonly IContactRepository _repository;
        private readonly ILogger<ContactController> _logger;

        public ContactController(IContactRepository repository, ILogger<ContactController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public IActionResult Teaser(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var contact = _repository.GetContactById(id);
            if (contact.IsNull())
            {
                return NotFound();
            }

            return PartialView("_ContactTeaser", contact);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            _logger.LogInformation($"DELETE request for Contact with ID '{id}'");
            if (id <= 0)
            {
                return NotFound();
            }

            var success = _repository.DeleteContact(id);

            if (success)
            {
                return Json(new {Success = true, Message = SuccessfullyDeletedMessage});
            }

            return Json(new {Success = false, Message = ContactCannotBeDeleted});
        }

        public IActionResult Add()
        {
            var contact = new Contact();

            return View(contact);
        }

        [HttpPost]
        public IActionResult Add(Contact contact)
        {
            _logger.LogInformation($"ADD request for Contact '{contact.FullName}'");
            if (contact.Id > 0)
            {
                return BadRequest();
            }

            var contactFromRepository = _repository.AddContact(contact);

            if (contactFromRepository.IsNull())
            {
                return BadRequest();
            }

            return RedirectToAction("Details", "Dashboard", new {id = contactFromRepository.Id});
        }

        public IActionResult Favorite(int id)
        {
            _logger.LogInformation($"FAVORITE request for Contact with ID '{id}'");
            if (id <= 0)
            {
                return NotFound();
            }

            var contact = _repository.GetContactById(id);
            if (contact.IsNull())
            {
                return NotFound();
            }

            contact.IsFavorite = !contact.IsFavorite;
            _repository.SaveContact(contact);

            return ViewComponent("ContactList");
        }
    }
}