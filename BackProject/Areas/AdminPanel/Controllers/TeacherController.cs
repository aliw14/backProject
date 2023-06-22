using BackProject.Areas.AdminPanel.Data;
using BackProject.DataAccessLayer;
using BackProject.DataAccessLayer.Entities;
using BackProject.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Files = System.IO.File;

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
            if (id == null)
            {
                return BadRequest();
            }

            var teachers = await _dbContext.Teachers.FindAsync(id);

            if (teachers == null)
            {
                return NotFound();
            }

            return View(teachers);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Teacher teachers)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var isExist = await _dbContext.Teachers.AnyAsync(x => x.Name.ToUpper() == teachers.Name.ToUpper());

            if (isExist)
            {
                ModelState.AddModelError("Title", "Bu Teacher Artıq Mövcuddur");
                return View();
            }

            if (!teachers.Photo.ContentType.Contains("image"))
            {
                ModelState.AddModelError("Image", "Şəkil seçməlisiniz!");
                return View();
            }

            if (teachers.Photo.Length > 1024 * 1024)
            {
                ModelState.AddModelError("Image", "Şəkil 1mb-dan çox olmamalıdır");
                return View();
            }

            var pathTeacher = Path.Combine(Constants.ImagePath, "teacher");
            var pathGuid = $"{Guid.NewGuid()}-{teachers.Photo.FileName}";

            var path = Path.Combine(pathTeacher, pathGuid);
            var pathUrl = Path.Combine("img", "teacher", pathGuid);

            var fs = new FileStream(path, FileMode.CreateNew);

            teachers.ImageUrl = pathUrl;

            await teachers.Photo.CopyToAsync(fs);
            fs.Close();

            await _dbContext.Teachers.AddAsync(teachers);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var teachers = await _dbContext.Teachers.FindAsync(id);

            if (teachers == null) return NotFound();

            var newPath = teachers.ImageUrl.Remove(0, 4);
            var path = Path.Combine(Constants.ImagePath, newPath);

            if (Files.Exists(path))
            {
                Files.Delete(path);
            }

            _dbContext.Teachers.Remove(teachers);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();

            var teachers = await _dbContext.Teachers.FindAsync(id);

            if (teachers == null) return NotFound();

            return View(teachers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Teacher teacher)
        {
            if (id == null) return NotFound();

            if (id != teacher.Id) return BadRequest();


            var existTeacher = await _dbContext.Teachers.FindAsync(id);

            existTeacher.Name = teacher.Name;

            existTeacher.Speciality = teacher.Speciality;

            var trimmedName = existTeacher.ImageUrl.Remove(0, 4);
            var pathForDelete = Path.Combine(Constants.ImagePath, trimmedName);

            if (Files.Exists(pathForDelete))
            {
                Files.Delete(pathForDelete);
            }

            var isExist = await _dbContext.Teachers.AnyAsync(x => x.Name.ToUpper() == teacher.Name.ToUpper() && x.Id != id);

            if (isExist)
            {
                ModelState.AddModelError("Title", "Bu Teacher Artıq Mövcuddur");
                return View();
            }

            var path = Path.Combine(Constants.ImagePath, "teacher");
            var fileName = await teacher.Photo.GenerateFile(path);
            existTeacher.ImageUrl = $"img/teacher/{fileName}";

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}

