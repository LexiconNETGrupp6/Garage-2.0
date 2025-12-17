using Microsoft.AspNetCore.Mvc;

namespace Garage_2._0.Controllers
{
    public class GarageController : Controller
    {        
        public IActionResult Index()
        {
            return View();
        }
    }
}
