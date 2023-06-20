using System;
using Microsoft.AspNetCore.Identity;

namespace BackProject.Data
{
	public class LocalizeIdentityError : IdentityErrorDescriber
	{
        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError
            {
                Description = "Email təkrarlana bilməz",
                Code = "101"
            };
        }
    }
}

