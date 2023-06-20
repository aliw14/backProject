using System;
using BackProject.DataAccessLayer.Entities;
using BackProject.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BackProject.Controllers
{
    public class AboutController : Controller
    {
        private readonly AppDbContext _dbcontext;

        public AboutController(AppDbContext appDbContext)
        {
            _dbcontext = appDbContext;
        }

        public IActionResult Index()
        {
            var NoticeBoards = _dbcontext.NoticeBoards.Take(6).ToList();

            var sessionTwos = _dbcontext.SessionTwos.ToList();

            var Teachers = _dbcontext.Teachers.Take(4).ToList();

            AboutViewModel viewModel = new AboutViewModel
            {
                NoticeBoards = NoticeBoards,
                SessionTwos=sessionTwos,
                Teachers=Teachers
            };

            return View(viewModel);
        }
    }

}

