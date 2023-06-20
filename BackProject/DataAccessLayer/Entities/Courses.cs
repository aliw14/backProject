﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackProject.DataAccessLayer.Entities
{
	public class Courses
	{
		public int Id { get; set; }

		[MaxLength(300)]

		public string? ImageUrl { get; set; }

		public string Title { get; set; }

		public string Description { get; set; }

		public string Button { get; set; }

        [NotMapped]

        public IFormFile Photo { get; set; }
    }
}

