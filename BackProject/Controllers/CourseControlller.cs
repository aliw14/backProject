using System;
using BackProject.DataAccessLayer.Entities;
using BackProject.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BackProject.Controllers
{
    public class CourseController : Controller
    {
        private readonly AppDbContext _dbcontext;

        public CourseController(AppDbContext appDbContext)
        {
            _dbcontext = appDbContext;
        }

        public IActionResult Index()
        {
            var courses = _dbcontext.Courses.ToList();

            CourseViewModel viewModel = new CourseViewModel
            {
                Courses = courses
            };

            return View(viewModel);
        }

    }

}

