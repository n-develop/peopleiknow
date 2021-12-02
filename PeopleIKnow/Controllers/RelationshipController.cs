using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PeopleIKnow.DataAccess.Repositories;
using PeopleIKnow.Models;

namespace PeopleIKnow.Controllers
{
    [Authorize(Roles = "user")]
    public class RelationshipController : Controller
    {
        private readonly IRepository<Relationship> _repository;
        private readonly ILogger<RelationshipController> _logger;

        public RelationshipController(IRepository<Relationship> repository, ILogger<RelationshipController> logger)
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

            return View("Editor", relationship);
        }

        [HttpPost]
        public async Task<ActionResult> Add(Relationship relationship)
        {
            if (relationship == null)
            {
                return BadRequest();
            }

            _logger.LogInformation(
                $"ADD request for relationship '{relationship.Type}:{relationship.Person}' on contact with ID '{relationship.ContactId}'");

            await _repository.AddAsync(relationship);

            return RedirectToAction("Details", "Dashboard", new { id = relationship.ContactId });
        }

        public async Task<ActionResult> Edit(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var relationship = await _repository.GetByIdAsync(id);
            if (relationship is null)
            {
                return NotFound();
            }

            return View("Editor", relationship);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Relationship relationship)
        {
            if (relationship == null)
            {
                return BadRequest();
            }

            _logger.LogInformation(
                $"EDIT request for relationship ({relationship.Id}) '{relationship.Type}:{relationship.Person}' on contact with ID '{relationship.ContactId}'");

            await _repository.UpdateAsync(relationship);

            return RedirectToAction("Details", "Dashboard", new { id = relationship.ContactId });
        }

        public async Task<ActionResult> Delete(int id)
        {
            _logger.LogInformation($"DELETE request for relationship with ID '{id}'");
            if (id <= 0)
            {
                return NotFound();
            }

            var relationship = await _repository.GetByIdAsync(id);
            if (relationship is null)
            {
                return NotFound();
            }

            await _repository.DeleteAsync(relationship);

            return RedirectToAction("Details", "Dashboard", new { id = relationship.ContactId });
        }
    }
}