using DRAWeb.App.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DRAWeb.App.Controllers
{
    public class ErrorController : BaseController
    {
        public IActionResult PageNotFound()
        {
            return View();
        }

        public IActionResult ServerError()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
