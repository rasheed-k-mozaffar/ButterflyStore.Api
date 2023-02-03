using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;

namespace ButterflyStore.Client.AuthConfiguration
{
	public class JwtAuthenticationStateProvider : AuthenticationStateProvider
	{
		private readonly ILocalStorageService _localStorage;

		public JwtAuthenticationStateProvider(ILocalStorageService localStorage)
		{
			_localStorage = localStorage;
		}

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
			//Check if an access token exists in the local storage.
            if(await _localStorage.ContainKeyAsync("access_token"))
			{
                //The user is logged in this case , and there's an access token in the local storage
                var jwtToken = await _localStorage.GetItemAsStringAsync("access_token");
                var tokenHandler = new JwtSecurityTokenHandler();

                //Read the JWT Token
                var token = tokenHandler.ReadJwtToken(jwtToken);

                //Create a claims identity for the user using the claims inside the JWT and select Bearer as the authentication type.
                var identity = new ClaimsIdentity(token.Claims, "Bearer");

                //Create a claims principal from the claims identity.
                var user = new ClaimsPrincipal(identity);

                //Create an Authentication State for the User.
                var authState = new AuthenticationState(user);

                //Rise an event to notify the app that the auth state has changed.
                NotifyAuthenticationStateChanged(Task.FromResult(authState));

                return authState;
            }

			//No access token , means no identity and the user isn't logged in.
			return new AuthenticationState(new ClaimsPrincipal());
        }
    }
}

