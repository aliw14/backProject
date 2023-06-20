using BackProject.Areas.AdminPanel.Data;
using BackProject.DataAccessLayer;
using BackProject.DataAccessLayer.Entities;
using BackProject.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Files = System.IO.File;

namespace BackProject.Areas.AdminPanel.Controllers
{
    public class EventController : AdminController
    {
        private readonly AppDbContext _dbContext;

        public EventController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var events = await _dbContext.Events.ToListAsync();

            return View(events);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var events = await _dbContext.Events.FindAsync(id);

            if (events == null)
            {
                return NotFound();
            }

            return View(events);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Event events)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var isExist = await _dbContext.Events.AnyAsync(x => x.Title.ToUpper() == events.Title.ToUpper());

            if (isExist)
            {
                ModelState.AddModelError("Title", "Bu Kurs Artıq Mövcuddur");
                return View();
            }

            if (!events.Photo.ContentType.Contains("image"))
            {
                ModelState.AddModelError("Image", "Şəkil seçməlisiniz!");
                return View();
            }

            if (events.Photo.Length > 1024 * 1024)
            {
                ModelState.AddModelError("Image", "Şəkil 1mb-dan çox olmamalıdır");
                return View();
            }

            var pathEvent = Path.Combine(Constants.ImagePath, "event");
            var pathGuid = $"{Guid.NewGuid()}-{events.Photo.FileName}";

            var path = Path.Combine(pathEvent, pathGuid);
            var pathUrl = Path.Combine("img", "event", pathGuid);

            var fs = new FileStream(path, FileMode.CreateNew);

            events.ImageUrl = pathUrl;

            await events.Photo.CopyToAsync(fs);
            fs.Close();

            await _dbContext.Events.AddAsync(events);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var events = await _dbContext.Events.FindAsync(id);

            if (events == null) return NotFound();

            var newPath = events.ImageUrl.Remove(0, 4);
            var path = Path.Combine(Constants.ImagePath, newPath);

            if (Files.Exists(path))
            {
                Files.Delete(path);
            }

            _dbContext.Events.Remove(events);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();

            var events = await _dbContext.Events.FindAsync(id);

            if (events == null) return NotFound();

            return View(events);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Event events)
        {
            if (id == null) return NotFound();

            if (id != events.Id) return BadRequest();


            var existEvent = await _dbContext.Events.FindAsync(id);

            existEvent.Title = events.Title;

            existEvent.Time = events.Time;

            var trimmedName = existEvent.ImageUrl.Remove(0, 4);
            var pathForDelete = Path.Combine(Constants.ImagePath, trimmedName);

            if (Files.Exists(pathForDelete))
            {
                Files.Delete(pathForDelete);
            }

            var isExist = await _dbContext.Events.AnyAsync(x => x.Title.ToUpper() == events.Title.ToUpper() && x.Id != id);

            if (isExist)
            {
                ModelState.AddModelError("Title", "Bu Kurs Artıq Mövcuddur");
                return View();
            }

            var path = Path.Combine(Constants.ImagePath, "event");
            var fileName = await events.Photo.GenerateFile(path);
            existEvent.ImageUrl = $"img/event/{fileName}";

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}

