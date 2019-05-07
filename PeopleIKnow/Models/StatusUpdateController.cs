using Microsoft.AspNetCore.Mvc;
using PeopleIKnow.Repositories;

namespace PeopleIKnow.Models
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
                ContactId = contactId
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