using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PeopleIKnow.Models;
using PeopleIKnow.Repositories;

namespace PeopleIKnow.Controllers
{
    public class RelationshipController : Controller
    {
        private readonly IContactRepository _repository;
        private readonly ILogger<RelationshipController> _logger;

        public RelationshipController(IContactRepository repository, ILogger<RelationshipController> logger)
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

            var relationship = new Relationship()
            {
                ContactId = contactId
            };

            return View(relationship);
        }

        [HttpPost]
        public ActionResult Add(Relationship relationship)
        {
            if (relationship == null)
            {
                return BadRequest();
            }

            _logger.LogInformation(
                $"ADD request for relationship '{relationship.Type}:{relationship.Person}' on contact with ID '{relationship.ContactId}'");

            _repository.AddRelationship(relationship);

            return RedirectToAction("Details", "Dashboard", new {id = relationship.ContactId});
        }

        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var relationship = _repository.GetRelationshipById(id);
            if (relationship.IsNull())
            {
                return NotFound();
            }

            return View(relationship);
        }

        [HttpPost]
        public ActionResult Edit(Relationship relationship)
        {
            if (relationship == null)
            {
                return BadRequest();
            }

            _logger.LogInformation(
                $"EDIT request for relationship ({relationship.Id}) '{relationship.Type}:{relationship.Person}' on contact with ID '{relationship.ContactId}'");

            _repository.UpdateRelationship(relationship);

            return RedirectToAction("Details", "Dashboard", new {id = relationship.ContactId});
        }

        public ActionResult Delete(int id)
        {
            _logger.LogInformation($"DELETE request for relationship with ID '{id}'");
            if (id <= 0)
            {
                return NotFound();
            }

            var relationship = _repository.GetRelationshipById(id);
            if (relationship.IsNull())
            {
                return NotFound();
            }

            _repository.DeleteRelationship(relationship);

            return RedirectToAction("Details", "Dashboard", new {id = relationship.ContactId});
        }
    }
}