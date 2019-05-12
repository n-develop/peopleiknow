using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PeopleIKnow.Models;
using PeopleIKnow.Repositories;

namespace PeopleIKnow.Controllers
{
    public class TelephoneController : Controller
    {
        private readonly IContactRepository _repository;

        public TelephoneController(IContactRepository repository)
        {
            _repository = repository;
        }

        public ActionResult Add(int contactId)
        {
            if (contactId <= 0)
            {
                return NotFound();
            }

            var telephoneNumber = new TelephoneNumber()
            {
                ContactId = contactId
            };

            return View(telephoneNumber);
        }

        [HttpPost]
        public ActionResult Add(TelephoneNumber telephoneNumber)
        {
            if (telephoneNumber == null)
            {
                return BadRequest();
            }

            _repository.AddTelephoneNumber(telephoneNumber);

            return RedirectToAction("Details", "Dashboard", new {id = telephoneNumber.ContactId});
        }

        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var telephoneNumber = _repository.GetTelephoneNumberById(id);
            if (telephoneNumber.IsNull())
            {
                return NotFound();
            }

            _repository.DeleteTelephoneNumber(telephoneNumber);

            return RedirectToAction("Details", "Dashboard", new {id = telephoneNumber.ContactId});
        }
    }
}