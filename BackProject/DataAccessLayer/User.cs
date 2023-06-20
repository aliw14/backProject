using System;
using Microsoft.AspNetCore.Identity;

namespace BackProject.DataAccessLayer
{
	public class User : IdentityUser
	{
		public string? FullName { get; set; }
	}
}

