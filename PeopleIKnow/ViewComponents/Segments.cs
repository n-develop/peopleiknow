using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace PeopleIKnow.ViewComponents
{
    public class Segments : ViewComponent
    {
        public Segments()
        {
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            await Task.Delay(1);
            return View();
        }
    }
}