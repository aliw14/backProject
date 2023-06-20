using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BackProject.Models;
using BackProject.DataAccessLayer.Entities;
using BackProject.ViewModels;

namespace BackProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _dbcontext;

        public HomeController(AppDbContext appDbContext)
        {
            _dbcontext = appDbContext;
        }

        public IActionResult Index()
        {
            var courses = _dbcontext.Courses.Take(3).ToList();

            var blogs = _dbcontext.Blogs.Take(3).ToList();

            var NoticeBoards = _dbcontext.NoticeBoards.ToList();

            var NoticeBoards2 = _dbcontext.NoticeBoard2s.ToList();

            var SessionOne = _dbcontext.SessionOnes.ToList();

            var SessionTwo = _dbcontext.SessionTwos.ToList();

            var Event = _dbcontext.Events.Take(4).ToList();


            HomeViewModel viewModel = new HomeViewModel
            {
                Courses = courses,
                Blogs=blogs,
                NoticeBoards=NoticeBoards,
                NoticeBoard2s=NoticeBoards2,
                SessionOnes=SessionOne,
                SessionTwos=SessionTwo,
                Events=Event
            };

            return View(viewModel);
        }

        public IActionResult Search(string searchedProduct)
        {
            if (string.IsNullOrEmpty(searchedProduct)) return NoContent();


            var products = _dbcontext.Teachers.Where(x => x.Name.Contains(searchedProduct)).ToList();

            return PartialView("_searchedProdustPartial", products);
        }
    }


}






