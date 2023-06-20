using System;
using Microsoft.AspNetCore.Mvc;

namespace BackProject.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

