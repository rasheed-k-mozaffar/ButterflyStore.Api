using System;
using ButterflyStore.Client.Exceptions;
using Microsoft.AspNetCore.Components;


namespace ButterflyStore.Client.Pages.Auth
{
	public partial class Register : ComponentBase
	{
		[Inject]
		private HttpClient HttpClient { get; set; } = null!;

        [Inject]
		private NavigationManager NavigationManager { get; set; } = null!;

		[Inject]
		private IAuthService AuthService { get; set; } = null!;

		//Our Form Model
		private RegisterUserDto model = new();

		//This will hold the error message returned from the server in case of failure
		private string? _errorMessage = string.Empty;

		//This will prevent the user from clicking again and again on the
		//register button to avoid concurrent requests by the same user.
		private bool _isBusy = false;

		private async Task RegisterUser()
		{
			//Disable the buttons here.
			_isBusy = true;

			try
			{
				await AuthService.RegisterUserAsync(model);

				NavigationManager.NavigateTo("/auth/login");
            }

            catch(ApiAuthException ex)
			{
				_errorMessage = ex.ErrorResponse.Message;

				Console.WriteLine(_errorMessage);
			}

			_isBusy = false;


		}
		
	}
}

