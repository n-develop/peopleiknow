using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PeopleIKnow.Models;
using PeopleIKnow.Repositories;

namespace PeopleIKnow.Controllers
{
    public class CommonActivityController : Controller
    {
        private readonly IContactRepository _repository;
        private readonly ILogger<CommonActivityController> _logger;

        public CommonActivityController(IContactRepository repository, ILogger<CommonActivityController> logger)
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

            var commonActivity = new CommonActivity
            {
                ContactId = contactId,
                Date = DateTime.Now
            };

            return View(commonActivity);
        }

        [HttpPost]
        public ActionResult Add(CommonActivity commonActivity)
        {
            if (commonActivity == null)
            {
                return BadRequest();
            }

            _logger.LogInformation("ADD request for activity '{Description}' on contact with ID '{ContactId}'",
                commonActivity.Description, commonActivity.ContactId);

            _repository.AddCommonActivity(commonActivity);

            return RedirectToAction("Details", "Dashboard", new {id = commonActivity.ContactId});
        }

        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var commonActivity = _repository.GetCommonActivityById(id);
            if (commonActivity.IsNull())
            {
                return NotFound();
            }

            return View(commonActivity);
        }

        [HttpPost]
        public ActionResult Edit(CommonActivity commonActivity)
        {
            if (commonActivity == null)
            {
                return BadRequest();
            }

            _logger.LogInformation(
                "EDIT request for common activity '{Description}' on contact with ID '{ContactId}'",
                commonActivity.Description, commonActivity.ContactId);

            _repository.UpdateCommonActivity(commonActivity);

            return RedirectToAction("Details", "Dashboard", new {id = commonActivity.ContactId});
        }

        public ActionResult Delete(int id)
        {
            _logger.LogInformation("DELETE request for common activity with ID '{Id}'", id);
            if (id <= 0)
            {
                return NotFound();
            }

            var commonActivity = _repository.GetCommonActivityById(id);
            if (commonActivity.IsNull())
            {
                return NotFound();
            }

            _repository.DeleteCommonActivity(commonActivity);

            return RedirectToAction("Details", "Dashboard", new {id = commonActivity.ContactId});
        }
    }
}