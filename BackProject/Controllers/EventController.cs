using System;
using BackProject.DataAccessLayer.Entities;
using BackProject.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackProject.Controllers
{
    public class EventController : Controller
    {
        private readonly AppDbContext _dbcontext;

        public EventController(AppDbContext appDbContext)
        {
            _dbcontext = appDbContext;
        }

        public IActionResult Index()
        {
            var Events = _dbcontext.Events.ToList();

            EventViewModel viewModel = new EventViewModel
            {
                Events = Events
            };

            return View(viewModel);
        }

    }

}

