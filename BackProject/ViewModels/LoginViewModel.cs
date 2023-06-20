using System;
using System.ComponentModel.DataAnnotations;

namespace BackProject.ViewModels
{
	public class LoginViewModel
	{
		[Required]

		public string Username { get; set; }

		[DataType(DataType.Password)]

		public string Password { get; set; }
	}
}

