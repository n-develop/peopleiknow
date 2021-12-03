using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PeopleIKnow.DataAccess.Repositories;
using PeopleIKnow.Models;

namespace PeopleIKnow.Controllers
{
    [Authorize(Roles = "user")]
    public class StatusUpdateController : Controller
    {
        private readonly IRepository<StatusUpdate> _repository;
        private readonly ILogger<StatusUpdateController> _logger;

        public StatusUpdateController(IRepository<StatusUpdate> repository, ILogger<StatusUpdateController> logger)
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

            var statusUpdate = new StatusUpdate
            {
                ContactId = contactId,
                Created = DateTime.Now
            };

            return View("Editor", statusUpdate);
        }

        [HttpPost]
        public async Task<ActionResult> Add(StatusUpdate statusUpdate)
        {
            if (statusUpdate == null)
            {
                return BadRequest();
            }

            _logger.LogInformation("ADD request for status update '{StatusText}' on contact with ID '{ContactId}'",
                statusUpdate.StatusText, statusUpdate.ContactId);

            await _repository.AddAsync(statusUpdate);

            return RedirectToAction("Details", "Contact", new { id = statusUpdate.ContactId });
        }

        public async Task<ActionResult> Edit(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var statusUpdate = await _repository.GetByIdAsync(id);
            if (statusUpdate is null)
            {
                return NotFound();
            }

            return View("Editor", statusUpdate);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(StatusUpdate statusUpdate)
        {
            if (statusUpdate == null)
            {
                return BadRequest();
            }

            _logger.LogInformation("EDIT request for status update '{StatusText}' on contact with ID '{ContactId}'",
                statusUpdate.StatusText, statusUpdate.ContactId);

            await _repository.UpdateAsync(statusUpdate);

            return RedirectToAction("Details", "Contact", new { id = statusUpdate.ContactId });
        }

        public async Task<ActionResult> Delete(int id)
        {
            _logger.LogInformation("DELETE request for status update with ID '{Id}'", id);
            if (id <= 0)
            {
                return NotFound();
            }

            var statusUpdate = await _repository.GetByIdAsync(id);
            if (statusUpdate is null)
            {
                return NotFound();
            }

            await _repository.DeleteAsync(statusUpdate);

            return RedirectToAction("Details", "Contact", new { id = statusUpdate.ContactId });
        }
    }
}