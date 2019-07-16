using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PeopleIKnow.Models;
using PeopleIKnow.Repositories;

namespace PeopleIKnow.Controllers
{
    public class StatusUpdateController : Controller
    {
        private readonly IContactRepository _repository;
        private readonly ILogger<StatusUpdateController> _logger;

        public StatusUpdateController(IContactRepository repository, ILogger<StatusUpdateController> logger)
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

            return View(statusUpdate);
        }

        [HttpPost]
        public ActionResult Add(StatusUpdate statusUpdate)
        {
            if (statusUpdate == null)
            {
                return BadRequest();
            }

            _logger.LogInformation(
                $"ADD request for status update '{statusUpdate.StatusText}' on contact with ID '{statusUpdate.ContactId}'");

            _repository.AddStatusUpdate(statusUpdate);

            return RedirectToAction("Details", "Dashboard", new {id = statusUpdate.ContactId});
        }

        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var statusUpdate = _repository.GetStatusUpdateById(id);
            if (statusUpdate == null)
            {
                return NotFound();
            }

            return View(statusUpdate);
        }

        [HttpPost]
        public ActionResult Edit(StatusUpdate statusUpdate)
        {
            if (statusUpdate == null)
            {
                return BadRequest();
            }

            _logger.LogInformation(
                $"EDIT request for status update '{statusUpdate.StatusText}' on contact with ID '{statusUpdate.ContactId}'");

            _repository.UpdateStatusUpdate(statusUpdate);

            return RedirectToAction("Details", "Dashboard", new {id = statusUpdate.ContactId});
        }

        public ActionResult Delete(int id)
        {
            _logger.LogInformation($"DELETE request for status update with ID '{id}'");
            if (id <= 0)
            {
                return NotFound();
            }

            var statusUpdate = _repository.GetStatusUpdateById(id);
            if (statusUpdate.IsNull())
            {
                return NotFound();
            }

            _repository.DeleteStatusUpdate(statusUpdate);

            return RedirectToAction("Details", "Dashboard", new {id = statusUpdate.ContactId});
        }
    }
}