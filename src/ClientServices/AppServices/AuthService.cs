using System;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using ButterflyStore.Client.Exceptions;
using ButterflyStore.Client.Services.Contracts;
using ButterflyStore.Shared;
using ButterflyStore.Shared.ApiResponses;

namespace ButterflyStore.Client.Services.AppServices
{
	public class AuthService : IAuthService
	{
        private readonly HttpClient _httpClient;

		public AuthService(HttpClient httpClient)
		{
            _httpClient = httpClient;
		}

        public async Task RegisterUserAsync(RegisterUserDto model)
        {
            //Make a POST request for the register endpoint with the model as JSON.
            var response = await _httpClient.PostAsJsonAsync("/api/v1/auth/register", model);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadFromJsonAsync<ApiAuthResponse>();
                throw new ApiAuthException(errorMessage!, null);
            }

        }

        public async Task<string> LoginUserAsync(LoginUserDto model)
        {
            //Make a POST request for the login endpoint with the model as JSON.
            var response = await _httpClient.PostAsJsonAsync("/api/v1/auth/login", model);

            if(!response.IsSuccessStatusCode)
            {
                //In case of failure , return the error message from the Api Auth Response.
                var errorMessage = await response.Content.ReadFromJsonAsync<ApiAuthResponse>();

                throw new ApiAuthException(errorMessage!, null);
            }
            else
            {
                //In case of success , return the JWT TOKEN.
                var result = await response.Content.ReadFromJsonAsync<ApiAuthResponse>();

                return result!.Message!;
            }
        }

    }
}

