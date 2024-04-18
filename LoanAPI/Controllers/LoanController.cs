using Microsoft.AspNetCore.Mvc;

namespace LoanAPI.Controllers
{
    public class LoanController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
