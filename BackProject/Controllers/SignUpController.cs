using System;
using Microsoft.AspNetCore.Mvc;

namespace BackProject.Controllers
{
	public class SignUpController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}

