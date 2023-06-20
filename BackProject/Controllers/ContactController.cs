using System;
using BackProject.DataAccessLayer.Entities;
using BackProject.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BackProject.Controllers
{
    public class ContactController : Controller
    {
        private readonly AppDbContext _dbcontext;

        public ContactController(AppDbContext appDbContext)
        {
            _dbcontext = appDbContext;
        }

        public IActionResult Index()
        {
            var contacts = _dbcontext.Contacts.ToList();

            ContactViewModel viewModel1 = new ContactViewModel
            {
                Contacts = contacts
            };

            return View(viewModel1);
        }
    }
}

