using System;
namespace BackProject.DataAccessLayer.Entities
{
	public class Footer
	{
		public int Id { get; set; }

		public string CreatedBy { get; set; }

		public ICollection<Information> Informations { get; set; }

		public ICollection<Links> Links { get; set; }

		public ICollection<Touch> Touches { get; set; }
	}
}