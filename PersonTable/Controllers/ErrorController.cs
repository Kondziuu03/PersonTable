using Microsoft.AspNetCore.Mvc;

namespace PersonTable.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Error(string message)
        {
            ViewBag.Message = message ?? "An unexpected error occurred.";
            return View();
        }
    }
}
