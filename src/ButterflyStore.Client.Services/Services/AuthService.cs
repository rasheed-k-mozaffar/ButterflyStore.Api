﻿using System;
using System.Net.Http.Json;
using Blazored.LocalStorage;
using ButterflyStore.Client.Services.Contracts;
using ButterflyStore.Shared;
using ButterflyStore.Shared.ApiResponses;

namespace ButterflyStore.Client.Services.Services
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
            }

        }


        public async Task<string> LoginUserAsync(LoginUserDto model)
        {
            //Make a POST request for the login endpoinnt with the model as JSON.
            var response = await _httpClient.PostAsJsonAsync("/api/v1/auth/login", model);

            if(!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Content.ReadFromJsonAsync<ApiAuthResponse>();

                return errorMessage!.Message!;
            }
            else
            {
                var result = await response.Content.ReadFromJsonAsync<ApiAuthResponse>();
                return result!.Message!;

            }
        }

    }
}

