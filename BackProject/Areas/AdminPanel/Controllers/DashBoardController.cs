using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace BackProject.Areas.AdminPanel.Controllers
{

    public class DashBoardController : AdminController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

