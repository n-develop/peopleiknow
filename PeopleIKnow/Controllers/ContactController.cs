using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PeopleIKnow.DataAccess.Repositories;
using PeopleIKnow.Models;
using PeopleIKnow.ViewModels;

namespace PeopleIKnow.Controllers
{
    [Authorize(Roles = "user")]
    public class ContactController : Controller
    {
        public static readonly string SuccessfullyDeletedMessage = "Successfully deleted";
        public static readonly string ContactCannotBeDeleted = "Contact cannot be deleted";
        private readonly IContactRepository _repository;
        private readonly ILogger<ContactController> _logger;
        private readonly IImageRepository _imageRepository;

        public ContactController(IContactRepository repository, ILogger<ContactController> logger,
            IImageRepository imageRepository)
        {
            _repository = repository;
            _logger = logger;
            _imageRepository = imageRepository;
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
                return Json(new { Success = true, Message = SuccessfullyDeletedMessage });
            }

            return Json(new { Success = false, Message = ContactCannotBeDeleted });
        }

        public IActionResult Add()
        {
            var contact = new Contact();

            return View(contact);
        }

        [HttpPost]
        public async Task<IActionResult> Add(Contact contact)
        {
            _logger.LogInformation($"ADD request for Contact '{contact.FullName}'");
            if (contact.Id > 0)
            {
                return BadRequest();
            }

            var contactFromRepository = await _repository.AddContact(contact);

            if (contactFromRepository.IsNull())
            {
                return BadRequest();
            }

            return RedirectToAction("Details", new { id = contactFromRepository.Id });
        }

        public async Task<IActionResult> Favorite(int id)
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
            await _repository.SaveContact(contact);

            return ViewComponent("ContactList");
        }

        public IActionResult Details(int id)
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

        [HttpPost]
        public async Task<IActionResult> Details(ContactUpdateViewModel contact)
        {
            if (contact == null)
            {
                return BadRequest();
            }

            var contactFromDb = _repository.GetContactById(contact.Id);
            if (contactFromDb.IsNull())
            {
                return NotFound();
            }

            contactFromDb.Address = contact.Address;
            contactFromDb.Birthday = contact.Birthday;
            contactFromDb.Employer = contact.Employer;
            contactFromDb.Lastname = contact.Lastname;
            contactFromDb.Firstname = contact.Firstname;
            contactFromDb.Middlename = contact.Middlename;
            contactFromDb.BusinessTitle = contact.BusinessTitle;
            contactFromDb.Tags = contact.Tags;

            if (contact.Image is { Length: > 0 })
            {
                contactFromDb.ImagePath = await _imageRepository.WriteFileToDiskAsync(contact.Image, contact.Id);
            }

            await _repository.SaveContact(contactFromDb);

            return Details(contact.Id);
        }
    }
}