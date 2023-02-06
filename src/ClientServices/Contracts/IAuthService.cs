using System;
using ButterflyStore.Shared;

namespace ButterflyStore.Client.Services.Contracts
{
	public interface IAuthService
	{
		Task RegisterUserAsync(RegisterUserDto model);
		Task<string> LoginUserAsync(LoginUserDto model);
	}
}

