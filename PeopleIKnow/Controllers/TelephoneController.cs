using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PeopleIKnow.Models;
using PeopleIKnow.Repositories;

namespace PeopleIKnow.Controllers
{
    public class TelephoneController : Controller
    {
        private readonly IContactRepository _repository;
        private readonly ILogger<TelephoneController> _logger;

        public TelephoneController(IContactRepository repository, ILogger<TelephoneController> logger)
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

            var telephoneNumber = new TelephoneNumber()
            {
                ContactId = contactId
            };

            return View(telephoneNumber);
        }

        [HttpPost]
        public ActionResult Add(TelephoneNumber telephoneNumber)
        {
            if (telephoneNumber == null)
            {
                return BadRequest();
            }

            _logger.LogInformation(
                $"ADD request for telephone number '{telephoneNumber.Type}:{telephoneNumber.Telephone}' on contact with ID '{telephoneNumber.ContactId}'");

            _repository.AddTelephoneNumber(telephoneNumber);

            return RedirectToAction("Details", "Dashboard", new {id = telephoneNumber.ContactId});
        }

        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var telephone = _repository.GetTelephoneNumberById(id);
            if (telephone.IsNull())
            {
                return NotFound();
            }

            return View(telephone);
        }

        [HttpPost]
        public ActionResult Edit(TelephoneNumber telephoneNumber)
        {
            if (telephoneNumber == null)
            {
                return BadRequest();
            }

            _logger.LogInformation(
                $"Edit request for telephone number '{telephoneNumber.Type}:{telephoneNumber.Telephone}' on contact with ID '{telephoneNumber.ContactId}'");

            _repository.UpdateTelephoneNumber(telephoneNumber);

            return RedirectToAction("Details", "Dashboard", new {id = telephoneNumber.ContactId});
        }

        public ActionResult Delete(int id)
        {
            _logger.LogInformation($"DELETE request for telephone number with ID '{id}'");
            if (id <= 0)
            {
                return NotFound();
            }

            var telephoneNumber = _repository.GetTelephoneNumberById(id);
            if (telephoneNumber.IsNull())
            {
                return NotFound();
            }

            _repository.DeleteTelephoneNumber(telephoneNumber);

            return RedirectToAction("Details", "Dashboard", new {id = telephoneNumber.ContactId});
        }
    }
}