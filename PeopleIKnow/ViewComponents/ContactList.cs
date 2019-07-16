using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PeopleIKnow.Repositories;

namespace PeopleIKnow.ViewComponents
{
    public class ContactList : ViewComponent
    {
        private readonly IContactRepository _repository;

        public ContactList(IContactRepository repository)
        {
            _repository = repository;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var contacts = await _repository.GetAllContacts();
            return View(contacts.OrderByDescending(contact => contact.IsFavorite).ThenBy(contact => contact.Lastname)
                .ToList());
        }
    }
}