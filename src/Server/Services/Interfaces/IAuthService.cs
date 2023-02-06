using System;
namespace ButterflyStore.Server.Services.Interfaces
{
	public interface IAuthService
	{
		Task<ApiAuthResponse> RegisterUserAsync(RegisterUserDto model);
		Task<ApiAuthResponse> LoginUserAsync(LoginUserDto model);
	}
}

