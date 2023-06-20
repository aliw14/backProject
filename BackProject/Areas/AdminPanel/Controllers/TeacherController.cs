using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackProject.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackProject.Areas.AdminPanel.Controllers
{

    public class TeacherController : AdminController
    {

        private readonly AppDbContext _dbContext;

        public TeacherController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var teachers = await _dbContext.Teachers.ToListAsync();

            return View(teachers);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var teacher = await _dbContext.Teachers.FirstOrDefaultAsync(x => x.Id == id);

            if (teacher == null) return NotFound();

            return View(teacher);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(Teacher teacher)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var isExist = await _dbContext.Teachers.AnyAsync(x => x.Name.ToLower().Equals(teacher.Name.ToLower()));

            if (isExist)
            {
                ModelState.AddModelError("Name", "Bu adda mellim movcuddur");

                return View();

            }


            await _dbContext.Teachers.AddAsync(teacher);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

             
        }

        [HttpPost]

        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var teacher = await _dbContext.Teachers.FindAsync(id);

            if (teacher == null) return NotFound();

            _dbContext.Teachers.Remove(teacher);

            await _dbContext.SaveChangesAsync();


            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();

            var teacher = await _dbContext.Teachers.FindAsync(id);

            if (teacher == null) return NotFound();

            return View(teacher);
        }


        [HttpPost]

        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Update(int? id,Teacher teacher)
        {
            if (id == null) return NotFound();

            if (id != teacher.Id) return BadRequest();

            var existTeacher = await _dbContext.Teachers.FindAsync(id);

            var existName = await _dbContext.Teachers.AnyAsync(x => x.Name.ToLower().Equals(teacher.Name.ToLower()) && x.Id != id);

            if (existName == true)
            {
                ModelState.AddModelError("Name", "Bu adda mellim movcuddur");
                return View(); 
            }

            existTeacher.Name = teacher.Name;

            existTeacher.Speciality = teacher.Speciality;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}

