using System;
using Microsoft.AspNetCore.Identity;

namespace ButterflyStore.Data.Models
{
	public class AppUser : IdentityUser
	{
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public Cart? Cart { get; set; }
	}
}

