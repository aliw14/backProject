using System;
using System.ComponentModel.DataAnnotations;

namespace BackProject.ViewModels
{
	public class RegisterViewModel
	{
		public string? Fullname { get; set; }
		[Required]

		public string UserName { get; set; }

		[DataType(DataType.EmailAddress)]

		public string Email { get; set; }

		[DataType(DataType.Password)]

		public string Password { get; set; }

        [Compare(nameof(Password))]

        [DataType(DataType.Password)]

		public string ConfirmPassword { get; set; }

	}
}

