using BackProject.Areas.AdminPanel.Data;
using BackProject.DataAccessLayer;
using BackProject.DataAccessLayer.Entities;
using BackProject.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Files = System.IO.File;

namespace BackProject.Areas.AdminPanel.Controllers
{
    public class CourseController : AdminController
    {
        private readonly AppDbContext _dbContext;

        public CourseController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var courses = await _dbContext.Courses.ToListAsync();

            return View(courses);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var course = await _dbContext.Courses.FindAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Courses course)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var isExist = await _dbContext.Courses.AnyAsync(x => x.Title.ToUpper() == course.Title.ToUpper());

            if (isExist)
            {
                ModelState.AddModelError("Title", "Bu Kurs Artıq Mövcuddur");
                return View();
            }

            if (!course.Photo.ContentType.Contains("image"))
            {
                ModelState.AddModelError("Image", "Şəkil seçməlisiniz!");
                return View();
            }

            if (course.Photo.Length > 1024 * 1024)
            {
                ModelState.AddModelError("Image", "Şəkil 1mb-dan çox olmamalıdır");
                return View();
            }

            var pathCourse = Path.Combine(Constants.ImagePath, "course");
            var pathGuid = $"{Guid.NewGuid()}-{course.Photo.FileName}";

            var path = Path.Combine(pathCourse, pathGuid);
            var pathUrl = Path.Combine("img", "course", pathGuid);

            var fs = new FileStream(path, FileMode.CreateNew);

            course.ImageUrl = pathUrl;

            await course.Photo.CopyToAsync(fs);
            fs.Close();

            await _dbContext.Courses.AddAsync(course);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var course = await _dbContext.Courses.FindAsync(id);

            if (course == null) return NotFound();

            var newPath = course.ImageUrl.Remove(0, 4);
            var path = Path.Combine(Constants.ImagePath, newPath);

            if (Files.Exists(path))
            {
                Files.Delete(path);
            }

            _dbContext.Courses.Remove(course);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();

            var course = await _dbContext.Courses.FindAsync(id);

            if (course == null) return NotFound();

            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Courses course)
        {
            if (id == null) return NotFound();

            if (id != course.Id) return BadRequest();


            var existCourse = await _dbContext.Courses.FindAsync(id);

            existCourse.Title = course.Title;

            existCourse.Description = course.Description;

            var trimmedName = existCourse.ImageUrl.Remove(0, 4);
            var pathForDelete = Path.Combine(Constants.ImagePath, trimmedName);

            if (Files.Exists(pathForDelete))
            {
                Files.Delete(pathForDelete);
            }

            var isExist = await _dbContext.Courses.AnyAsync(x => x.Title.ToUpper() == course.Title.ToUpper() && x.Id != id);

            if (isExist)
            {
                ModelState.AddModelError("Title", "Bu Kurs Artıq Mövcuddur");
                return View();
            }

            var path = Path.Combine(Constants.ImagePath, "course");
            var fileName = await course.Photo.GenerateFile(path);
            existCourse.ImageUrl = $"img/course/{fileName}";

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}

