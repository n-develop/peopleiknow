using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PeopleIKnow.DataAccess.Repositories;
using PeopleIKnow.Models;

namespace PeopleIKnow.Controllers
{
    [Authorize(Roles = "user")]
    public class TelephoneController : Controller
    {
        private readonly IRepository<TelephoneNumber> _repository;
        private readonly IContactRepository _contactRepository;
        private readonly ILogger<TelephoneController> _logger;

        public TelephoneController(IRepository<TelephoneNumber> repository,
            IContactRepository contactRepository,
            ILogger<TelephoneController> logger)
        {
            _repository = repository;
            _contactRepository = contactRepository;
            _logger = logger;
        }

        public ActionResult Add(int contactId)
        {
            if (contactId <= 0)
            {
                return NotFound();
            }

            var contact = _contactRepository.GetContactById(contactId);
            var telephoneNumber = new TelephoneNumber
            {
                Contact = contact,
                ContactId = contactId
            };

            return View("Editor", telephoneNumber);
        }

        [HttpPost]
        public async Task<ActionResult> Add(TelephoneNumber telephoneNumber)
        {
            if (telephoneNumber == null)
            {
                return BadRequest();
            }

            _logger.LogInformation(
                $"ADD request for telephone number '{telephoneNumber.Type}:{telephoneNumber.Telephone}' on contact with ID '{telephoneNumber.ContactId}'");

            await _repository.AddAsync(telephoneNumber);

            return RedirectToAction("Details", "Contact", new { id = telephoneNumber.ContactId });
        }

        public async Task<ActionResult> Edit(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var telephone = await _repository.GetByIdAsync(id);
            if (telephone is null)
            {
                return NotFound();
            }

            return View("Editor", telephone);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(TelephoneNumber telephoneNumber)
        {
            if (telephoneNumber == null)
            {
                return BadRequest();
            }

            _logger.LogInformation(
                $"Edit request for telephone number '{telephoneNumber.Type}:{telephoneNumber.Telephone}' on contact with ID '{telephoneNumber.ContactId}'");

            await _repository.UpdateAsync(telephoneNumber);

            return RedirectToAction("Details", "Contact", new { id = telephoneNumber.ContactId });
        }

        public async Task<ActionResult> Delete(int id)
        {
            _logger.LogInformation($"DELETE request for telephone number with ID '{id}'");
            if (id <= 0)
            {
                return NotFound();
            }

            var telephoneNumber = await _repository.GetByIdAsync(id);
            if (telephoneNumber is null)
            {
                return NotFound();
            }

            await _repository.DeleteAsync(telephoneNumber);

            return RedirectToAction("Details", "Contact", new { id = telephoneNumber.ContactId });
        }
    }
}