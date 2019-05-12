using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PeopleIKnow.Models;
using PeopleIKnow.Repositories;

namespace PeopleIKnow.Controllers
{
    public class RelationshipController : Controller
    {
        private readonly IContactRepository _repository;

        public RelationshipController(IContactRepository repository)
        {
            _repository = repository;
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

            _repository.AddRelationship(relationship);

            return RedirectToAction("Details", "Dashboard", new {id = relationship.ContactId});
        }

        public ActionResult Delete(int id)
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

            _repository.DeleteRelationship(relationship);

            return RedirectToAction("Details", "Dashboard", new {id = relationship.ContactId});
        }
    }
}