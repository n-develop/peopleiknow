using System.Threading.Tasks;
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
        private readonly IRepository<EmailAddress> _repository;
        private readonly ILogger<EmailController> _logger;

        public EmailController(IRepository<EmailAddress> repository, ILogger<EmailController> logger)
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
        public async Task<ActionResult> Add(EmailAddress mail)
        {
            if (mail == null)
            {
                return BadRequest();
            }

            _logger.LogInformation("ADD request for Email '{Email}' on contact with ID '{ContactId}'", mail.Email, mail.ContactId);

            await _repository.AddAsync(mail);

            return RedirectToAction("Details", "Dashboard", new {id = mail.ContactId});
        }

        public async Task<ActionResult> Edit(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var mail = await _repository.GetByIdAsync(id);
            if (mail is null)
            {
                return NotFound();
            }

            return View(mail);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(EmailAddress mail)
        {
            if (mail == null)
            {
                return BadRequest();
            }

            _logger.LogInformation("EDIT request for Email '{Email}' on contact with ID '{ContactId}'", mail.Email, mail.ContactId);

            await _repository.UpdateAsync(mail);

            return RedirectToAction("Details", "Dashboard", new {id = mail.ContactId});
        }

        public async Task<ActionResult> Delete(int id)
        {
            _logger.LogInformation("DELETE request for Email with ID '{Id}'", id);
            if (id <= 0)
            {
                return NotFound();
            }

            var mail = await _repository.GetByIdAsync(id);
            if (mail is null)
            {
                return NotFound();
            }

            await _repository.DeleteAsync(mail);

            return RedirectToAction("Details", "Dashboard", new {id = mail.ContactId});
        }
    }
}