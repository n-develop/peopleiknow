using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PeopleIKnow.Models;
using PeopleIKnow.Repositories;
using PeopleIKnow.Services;
using PeopleIKnow.ViewModels;

namespace PeopleIKnow.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IContactRepository _repository;
        private readonly IImageRepository _imageRepository;

        public DashboardController(IContactRepository repository, IImageRepository imageRepository)
        {
            _repository = repository;
            _imageRepository = imageRepository;
        }

        public IActionResult Index()
        {
            return View();
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

            if (contact.Image != null && contact.Image.Length > 0)
            {
                contactFromDb.ImagePath = await _imageRepository.WriteFileToDiskAsync(contact.Image, contact.Id);
            }

            _repository.SaveContact(contactFromDb);

            return Details(contact.Id);
        }

        public IActionResult ContactList()
        {
            return ViewComponent("ContactList");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}