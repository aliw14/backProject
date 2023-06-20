using System;
using BackProject.DataAccessLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BackProject.ViewComponents
{
	public class FooterViewComponent : ViewComponent
	{
		private readonly AppDbContext _dbcontext;

        public FooterViewComponent(AppDbContext dbContext)
		{
			_dbcontext = dbContext;
		}

		public IViewComponentResult Invoke()
		{
			var footer = _dbcontext.Footers.Include(x => x.Informations).Include(x => x.Links).Include(x => x.Touches).FirstOrDefault();

			return View(footer);
		}
	}
}

