using System;
using BackProject.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackProject.ViewComponents
{
	public class HeaderViewComponent :ViewComponent
	{
		private readonly AppDbContext _dbcontext;

        public HeaderViewComponent(AppDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

		public IViewComponentResult Invoke()
		{
			var header = _dbcontext.Headers.FirstOrDefault();

			return View(header);
		}
	}
}

