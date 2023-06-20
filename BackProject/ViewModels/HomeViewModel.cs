using System;
using BackProject.DataAccessLayer.Entities;

namespace BackProject.ViewModels
{
	public class HomeViewModel
	{
		public List<Courses>? Courses { get; set; }

		public List<Blog>? Blogs { get; set; }

        public List<NoticeBoard>? NoticeBoards { get; set; }

        public List<NoticeBoard2>? NoticeBoard2s { get; set; }

        public List<SessionOne>? SessionOnes { get; set; }

        public List<SessionTwo>? SessionTwos { get; set; }

        public List<Event>? Events { get; set; }


    }
}

