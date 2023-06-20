using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackProject.DataAccessLayer.Entities
{
	public class Event
	{
		public int Id { get; set; }

		public string? ImageUrl { get; set; }

		public string? Title { get; set; }

		public string? Time { get; set; }

		public string? Location { get; set; }

        [NotMapped]

        public IFormFile Photo { get; set; }

    }
}

