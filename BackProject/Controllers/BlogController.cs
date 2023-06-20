using System;
using BackProject.DataAccessLayer.Entities;
using BackProject.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BackProject.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _dbcontext;

        public BlogController(AppDbContext appDbContext)
        {
            _dbcontext = appDbContext;
        }

        public IActionResult Index()
        {
            var blogs = _dbcontext.Blogs.ToList();

            BlogViewModel viewModel1 = new BlogViewModel
            {
                Blogs = blogs
            };

            return View(viewModel1);
        }
    }

}

