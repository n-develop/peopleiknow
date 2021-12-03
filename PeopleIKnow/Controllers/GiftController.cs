using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PeopleIKnow.DataAccess.Repositories;
using PeopleIKnow.Models;

namespace PeopleIKnow.Controllers
{
    [Authorize(Roles = "user")]
    public class GiftController : Controller
    {
        private readonly ILogger<GiftController> _logger;
        private readonly IRepository<Gift> _repository;

        public GiftController(ILogger<GiftController> logger, IRepository<Gift> repository)
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

            var gift = new Gift
            {
                ContactId = contactId
            };

            return View("Editor", gift);
        }

        [HttpPost]
        public async Task<ActionResult> Add(Gift gift)
        {
            if (gift == null)
            {
                return BadRequest();
            }

            _logger.LogInformation("ADD request for Gift '{Descriptoin}' on contact with ID '{ContactId}'",
                gift.Description, gift.ContactId);

            await _repository.AddAsync(gift);

            return RedirectToAction("Details", "Contact", new { id = gift.ContactId });
        }

        public async Task<ActionResult> Edit(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var gift = await _repository.GetByIdAsync(id);
            if (gift is null)
            {
                return NotFound();
            }

            return View("Editor", gift);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(Gift gift)
        {
            if (gift == null)
            {
                return BadRequest();
            }

            _logger.LogInformation("EDIT request for Gift '{Description}' on contact with ID '{ContactId}'",
                gift.Description, gift.ContactId);

            await _repository.UpdateAsync(gift);

            return RedirectToAction("Details", "Contact", new { id = gift.ContactId });
        }

        public async Task<ActionResult> Delete(int id)
        {
            _logger.LogInformation("DELETE request for Gift with ID '{Id}'", id);
            if (id <= 0)
            {
                return NotFound();
            }

            var gift = await _repository.GetByIdAsync(id);
            if (gift is null)
            {
                return NotFound();
            }

            await _repository.DeleteAsync(gift);

            return RedirectToAction("Details", "Contact", new { id = gift.ContactId });
        }
    }
}