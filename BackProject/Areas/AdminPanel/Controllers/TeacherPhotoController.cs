using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackProject.Areas.AdminPanel.Data;
using BackProject.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackProject.Areas.AdminPanel.Controllers
{
    public class TeacherPhotoController : AdminController
    {
        private readonly AppDbContext _dbcontext;

        public TeacherPhotoController(AppDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<IActionResult> Index()
        {
            var teacherPhoto = await _dbcontext.Teachers.ToListAsync();

            return View(teacherPhoto);
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
                return View();

            if (teacher.Photo.ContentType.Contains("photo"))
            {
                ModelState.AddModelError("Photo", "sekil secin");
                return View();

            }

            if ( teacher.Photo.Length > 1024 * 1024)
            {
                ModelState.AddModelError("Photo", "1mb-dan cox olmalidir");
                return View();

            }

            var pathCourse = Path.Combine(Constants.ImagePath, "teacher");

            var pathGuid = $"{Guid.NewGuid()}-{teacher.Photo.FileName}";

            var path = Path.Combine(pathCourse, pathGuid);

            var fs = new FileStream(pathCourse, FileMode.CreateNew);

            teacher.ImageUrl = pathGuid;

            await teacher.Photo.CopyToAsync(fs);

            await _dbcontext.Teachers.AddAsync(teacher);

            await _dbcontext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}

