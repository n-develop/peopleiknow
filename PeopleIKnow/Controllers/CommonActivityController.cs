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
    public class CommonActivityController : Controller
    {
        private readonly IRepository<CommonActivity> _repository;
        private readonly ILogger<CommonActivityController> _logger;

        public CommonActivityController(IRepository<CommonActivity> repository,
            ILogger<CommonActivityController> logger)
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

            return View("Editor", commonActivity);
        }

        [HttpPost]
        public async Task<ActionResult> Add(CommonActivity commonActivity)
        {
            if (commonActivity == null)
            {
                return BadRequest();
            }

            _logger.LogInformation("ADD request for activity '{Description}' on contact with ID '{ContactId}'",
                commonActivity.Description, commonActivity.ContactId);

            await _repository.AddAsync(commonActivity);

            return RedirectToAction("Details", "Dashboard", new {id = commonActivity.ContactId});
        }

        public async Task<ActionResult> Edit(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var commonActivity = await _repository.GetByIdAsync(id);
            if (commonActivity is null)
            {
                return NotFound();
            }

            return View("Editor", commonActivity);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(CommonActivity commonActivity)
        {
            if (commonActivity == null)
            {
                return BadRequest();
            }

            _logger.LogInformation(
                "EDIT request for common activity '{Description}' on contact with ID '{ContactId}'",
                commonActivity.Description, commonActivity.ContactId);

            await _repository.UpdateAsync(commonActivity);

            return RedirectToAction("Details", "Dashboard", new {id = commonActivity.ContactId});
        }

        public async Task<ActionResult> Delete(int id)
        {
            _logger.LogInformation("DELETE request for common activity with ID '{Id}'", id);
            if (id <= 0)
            {
                return NotFound();
            }

            var commonActivity = await _repository.GetByIdAsync(id);
            if (commonActivity is null)
            {
                return NotFound();
            }

            await _repository.DeleteAsync(commonActivity);

            return RedirectToAction("Details", "Dashboard", new {id = commonActivity.ContactId});
        }
    }
}