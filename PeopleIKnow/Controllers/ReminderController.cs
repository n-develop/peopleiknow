using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PeopleIKnow.Models;
using PeopleIKnow.Repositories;

namespace PeopleIKnow.Controllers
{
    [Authorize]
    public class ReminderController : Controller
    {
        private readonly ILogger<ReminderController> _logger;
        private readonly IRepository<Reminder> _repository;

        public ReminderController(ILogger<ReminderController> logger, IRepository<Reminder> repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public ActionResult Add(int contactId)
        {
            if (contactId <= 0)
            {
                return NotFound();
            }

            var reminder = new Reminder
            {
                Date = DateTime.Today,
                ContactId = contactId
            };

            return View(reminder);
        }

        [HttpPost]
        public async Task<ActionResult> Add(Reminder reminder)
        {
            if (reminder == null)
            {
                return BadRequest();
            }

            _logger.LogInformation("ADD request for Reminder '{Description}' on contact with ID '{ContactId}'",
                reminder.Description, reminder.ContactId);

            await _repository.AddAsync(reminder);

            return RedirectToAction("Details", "Dashboard", new {id = reminder.ContactId});
        }

        public async Task<ActionResult> Edit(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var reminder = await _repository.GetByIdAsync(id);
            if (reminder is null)
            {
                return NotFound();
            }

            return View(reminder);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Reminder reminder)
        {
            if (reminder == null)
            {
                return BadRequest();
            }

            _logger.LogInformation("EDIT request for Reminder '{Description}' on contact with ID '{ContactId}'",
                reminder.Description, reminder.ContactId);

            await _repository.UpdateAsync(reminder);

            return RedirectToAction("Details", "Dashboard", new {id = reminder.ContactId});
        }

        public async Task<ActionResult> Delete(int id)
        {
            _logger.LogInformation("DELETE request for Reminder with ID '{Id}'", id);
            if (id <= 0)
            {
                return NotFound();
            }

            var reminder = await _repository.GetByIdAsync(id);
            if (reminder is null)
            {
                return NotFound();
            }

            await _repository.DeleteAsync(reminder);

            return RedirectToAction("Details", "Dashboard", new {id = reminder.ContactId});
        }
    }
}