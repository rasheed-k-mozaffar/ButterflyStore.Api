using System;
using Blazored.LocalStorage;
using ButterflyStore.Client.Exceptions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace ButterflyStore.Client.Pages.Auth
{
    public partial class Login : ComponentBase
    {
        [Inject]
        private HttpClient HttpClient { get; set; } = null!;

        [Inject]
        private NavigationManager NavigationManager { get; set; } = null!;

        [Inject]
        private IAuthService AuthService { get; set; } = null!;

        [Inject]
        private ILocalStorageService LocalStorage { get; set; } = null!;

        [Inject]
        private AuthenticationStateProvider AuthStateProvider { get; set; } = null!;
        //Our Form Model
        private LoginUserDto model = new();

        //This will hold the error message returned from the server in case of failure
        private string? _errorMessage = string.Empty;

        //This will prevent the user from clicking again and again on the
        //register button to avoid concurrent requests by the same user.
        private bool _isBusy = false;

        private async Task LoginUser()
        {
            //Disable the buttons here.
            _isBusy = true;

            try
            {
                //This call to the Auth Service will return a JWT token as a string in case the call was successful
                //if there was a token returne , then we will add it to the local storage for a key named "access_token"
                //and then update the authentication state to inform the app that this user is now authorized.
                var token = await AuthService.LoginUserAsync(model);

                await LocalStorage.SetItemAsStringAsync("access_token", token);

                await AuthStateProvider.GetAuthenticationStateAsync();

                NavigationManager.NavigateTo("/");
            }

            catch (ApiAuthException ex)
            {
                _errorMessage = ex.ErrorResponse.Message;
            }

            //Re enable the buttons here
            _isBusy = false;
        }

    }
}

