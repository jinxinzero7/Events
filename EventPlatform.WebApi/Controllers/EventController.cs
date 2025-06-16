using Microsoft.AspNetCore.Mvc;

namespace EventPlatform.WebApi.Controllers
{
    public class EventController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
