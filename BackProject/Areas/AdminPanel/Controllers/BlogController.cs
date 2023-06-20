using BackProject.Areas.AdminPanel.Data;
using BackProject.DataAccessLayer;
using BackProject.DataAccessLayer.Entities;
using BackProject.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Files = System.IO.File;

namespace BackProject.Areas.AdminPanel.Controllers
{
    public class BlogController : AdminController
    {
        private readonly AppDbContext _dbContext;

        public BlogController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var blogs = await _dbContext.Blogs.ToListAsync();

            return View(blogs);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var blogs = await _dbContext.Blogs.FindAsync(id);

            if (blogs == null)
            {
                return NotFound();
            }

            return View(blogs);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Blog blog)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var isExist = await _dbContext.Blogs.AnyAsync(x => x.Title.ToUpper() == blog.Title.ToUpper());

            if (isExist)
            {
                ModelState.AddModelError("Title", "Bu Kurs Artıq Mövcuddur");
                return View();
            }

            if (!blog.Photo.ContentType.Contains("image"))
            {
                ModelState.AddModelError("Image", "Şəkil seçməlisiniz!");
                return View();
            }

            if (blog.Photo.Length > 1024 * 1024)
            {
                ModelState.AddModelError("Image", "Şəkil 1mb-dan çox olmamalıdır");
                return View();
            }

            var pathBlog = Path.Combine(Constants.ImagePath, "blog");
            var pathGuid = $"{Guid.NewGuid()}-{blog.Photo.FileName}";

            var path = Path.Combine(pathBlog, pathGuid);
            var pathUrl = Path.Combine("img", "blog", pathGuid);

            var fs = new FileStream(path, FileMode.CreateNew);

            blog.ImageUrl = pathUrl;

            await blog.Photo.CopyToAsync(fs);
            fs.Close();

            await _dbContext.Blogs.AddAsync(blog);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var blog = await _dbContext.Blogs.FindAsync(id);

            if (blog == null) return NotFound();

            var newPath = blog.ImageUrl.Remove(0, 4);
            var path = Path.Combine(Constants.ImagePath, newPath);

            if (Files.Exists(path))
            {
                Files.Delete(path);
            }

            _dbContext.Blogs.Remove(blog);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();

            var blog = await _dbContext.Blogs.FindAsync(id);

            if (blog == null) return NotFound();

            return View(blog);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Blog blog)
        {
            if (id == null) return NotFound();

            if (id != blog.Id) return BadRequest();


            var existBlog = await _dbContext.Blogs.FindAsync(id);

            existBlog.Title = blog.Title;

            existBlog.Description = blog.Description;

            var trimmedName = existBlog.ImageUrl.Remove(0, 4);
            var pathForDelete = Path.Combine(Constants.ImagePath, trimmedName);

            if (Files.Exists(pathForDelete))
            {
                Files.Delete(pathForDelete);
            }

            var isExist = await _dbContext.Blogs.AnyAsync(x => x.Title.ToUpper() == blog.Title.ToUpper() && x.Id != id);

            if (isExist)
            {
                ModelState.AddModelError("Title", "Bu Kurs Artıq Mövcuddur");
                return View();
            }

            var path = Path.Combine(Constants.ImagePath, "blog");
            var fileName = await blog.Photo.GenerateFile(path);
            existBlog.ImageUrl = $"img/blog/{fileName}";

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
