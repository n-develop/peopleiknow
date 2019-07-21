using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PeopleIKnow.Models;
using PeopleIKnow.Repositories;
using PeopleIKnow.ViewModels;

namespace PeopleIKnow.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IContactRepository _repository;
        private readonly IHostingEnvironment _hostingEnvironment;

        public DashboardController(IContactRepository repository, IHostingEnvironment hostingEnvironment)
        {
            _repository = repository;
            _hostingEnvironment = hostingEnvironment;
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
                contactFromDb.ImagePath = await ReadImageFile(contact.Image, contact.Id);
            }

            _repository.SaveContact(contactFromDb);

            return Details(contact.Id);
        }

        private async Task<string> ReadImageFile(IFormFile formFile, int contactId)
        {
            var filename = contactId + formFile.FileName.Substring(formFile.FileName.LastIndexOf("."));
            var filePath = Path.Combine(_hostingEnvironment.WebRootPath, "images", "contacts", filename);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await formFile.CopyToAsync(stream);
            }

            return "images/contacts/" + filename;
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