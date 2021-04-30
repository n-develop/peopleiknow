using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PeopleIKnow.Models;
using PeopleIKnow.Repositories;

namespace PeopleIKnow.Controllers
{
    [Authorize]
    public class EmailController : Controller
    {
        private readonly IContactRepository _repository;
        private readonly ILogger<EmailController> _logger;

        public EmailController(IContactRepository repository, ILogger<EmailController> logger)
        {
            _repository = repository;
            _logger = logger;
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

            _logger.LogInformation($"ADD request for Email '{mail.Email}' on contact with ID '{mail.ContactId}'");

            _repository.AddEmail(mail);

            return RedirectToAction("Details", "Dashboard", new {id = mail.ContactId});
        }

        public ActionResult Edit(int id)
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

            return View(mail);
        }

        [HttpPost]
        public ActionResult Edit(EmailAddress mail)
        {
            if (mail == null)
            {
                return BadRequest();
            }

            _logger.LogInformation($"EDIT request for Email '{mail.Email}' on contact with ID '{mail.ContactId}'");

            _repository.UpdateEmail(mail);

            return RedirectToAction("Details", "Dashboard", new {id = mail.ContactId});
        }

        public ActionResult Delete(int id)
        {
            _logger.LogInformation($"DELETE request for Email with ID '{id}'");
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