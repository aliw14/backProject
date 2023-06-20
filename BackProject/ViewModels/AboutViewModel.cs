using System;
using BackProject.DataAccessLayer.Entities;

namespace BackProject.ViewModels
{
	public class AboutViewModel
	{
		public List<NoticeBoard>? NoticeBoards { get; set; }

		public List<SessionTwo>? SessionTwos { get; set; }

        public List<Teacher>? Teachers { get; set; }

    }
}

