using System;
using Microsoft.AspNetCore.Mvc;
using PeopleIKnow.Models;
using PeopleIKnow.Repositories;

namespace PeopleIKnow.Controllers
{
    public class StatusUpdateController : Controller
    {
        private readonly IContactRepository _repository;

        public StatusUpdateController(IContactRepository repository)
        {
            _repository = repository;
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

            _repository.AddStatusUpdate(statusUpdate);

            return RedirectToAction("Details", "Dashboard", new {id = statusUpdate.ContactId});
        }
    }
}