using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackProject.DataAccessLayer.Entities
{
	public class Teacher
	{
		public int Id { get; set; }

		[Required, MaxLength(30)]

		public string? Name { get; set; }

		[MaxLength(150)]

		public string? Speciality { get; set; }

		[MaxLength(300)]

        public string? ImageUrl { get; set; }

        [NotMapped]

        public IFormFile Photo { get; set; }

    }
}
