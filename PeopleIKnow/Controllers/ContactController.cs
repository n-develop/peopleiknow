using Microsoft.AspNetCore.Mvc;
using PeopleIKnow.Models;
using PeopleIKnow.Repositories;

namespace PeopleIKnow.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactRepository _repository;

        public ContactController(IContactRepository repository)
        {
            _repository = repository;
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

            return View(contact);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var success = _repository.DeleteContact(id);

            if (success)
            {
                return Json(new {Success = true, Message = "Successfully deleted"});
            }

            return Json(new {success = true, Message = "Contact cannot be deleted"});
        }

        public IActionResult Add()
        {
            var contact = new Contact();

            return View(contact);
        }

        [HttpPost]
        public IActionResult Add(Contact contact)
        {
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
            if (id <= 0)
            {
                return NotFound();
            }

            var contact = _repository.GetContactById(id);
            contact.IsFavorite = !contact.IsFavorite;
            _repository.SaveContact(contact);

            return ViewComponent("ContactList");
        }
    }
}