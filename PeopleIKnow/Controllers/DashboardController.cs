using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeopleIKnow.Models;
using PeopleIKnow.Repositories;

namespace PeopleIKnow.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IContactRepository _repository;

        public DashboardController(IContactRepository repository)
        {
            _repository = repository;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(int id)
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
        public ActionResult Details(Contact contact)
        {
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

            _repository.SaveContact(contactFromDb);

            return Details(contact.Id);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}