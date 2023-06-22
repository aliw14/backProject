using System;
using BackProject.DataAccessLayer.Entities;
using BackProject.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BackProject.Controllers
{
    public class TeacherController : Controller
    {
        private readonly AppDbContext _dbcontext;

        private readonly int _teacherCount;

        public TeacherController(AppDbContext appDbContext)
        {
            _dbcontext = appDbContext;

            _teacherCount = _dbcontext.Teachers.Count(); 
        }

        public IActionResult Index()
        {
            ViewBag.TeacherCount = _teacherCount;

            var Teachers = _dbcontext.Teachers.Take(4).ToList();

            TeacherViewModel viewModel1 = new TeacherViewModel
            {
                Teachers = Teachers
            };

            return View(viewModel1);
        }

        public IActionResult Search(string searchedTeacherName)
        {
            var searchedTeachers = _dbcontext.Teachers
                .Where(x => x.Name.ToLower().Contains(searchedTeacherName.ToLower()))
                .ToList();

            return PartialView("_SearchedTeacherPartial", searchedTeachers);
        }

        public IActionResult LoadTeacher(int skip)
        {
            if (skip >= _teacherCount) return BadRequest();

            var teachers = _dbcontext.Teachers.Skip(skip).Take(4).ToList();

            return PartialView("_TeacherPartial", teachers);
        }

        public IActionResult Details(int? id)
        {
            if (id is null) return NotFound();

            var teacher = _dbcontext.Teachers.FirstOrDefault(t => t.Id == id);

            return View(teacher);
        }
    }
}

