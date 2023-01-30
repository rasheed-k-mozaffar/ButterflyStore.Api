using System;
namespace ButterflyStore.Shared
{
	public class RegisterUserDto
	{
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string? Email { get; set; }
		public string? Password { get; set; }
		public string? PasswordConfirmation { get; set; }
	}
}

	