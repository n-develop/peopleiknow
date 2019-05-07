using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PeopleIKnow.Models;
using PeopleIKnow.Repositories;

namespace PeopleIKnow.Controllers
{
    public class EmailController : Controller
    {
        private readonly IContactRepository _repository;

        public EmailController(IContactRepository repository)
        {
            _repository = repository;
        }

        public ActionResult Add(int contactId)
        {
            if (contactId <= 0)
            {
                return NotFound();
            }

            var mail = new EmailAddress
            {
                ContactId = contactId
            };

            return View(mail);
        }

        [HttpPost]
        public ActionResult Add(EmailAddress mail)
        {
            if (mail == null)
            {
                return BadRequest();
            }

            _repository.AddEmail(mail);

            return RedirectToAction("Details", "Dashboard", new {id = mail.ContactId});
        }

        public ActionResult Delete(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var mail = _repository.GetEmailById(id);
            if (mail.IsNull())
            {
                return NotFound();
            }

            _repository.DeleteEmailAddress(mail);

            return RedirectToAction("Details", "Dashboard", new {id = mail.ContactId});
        }
    }
}