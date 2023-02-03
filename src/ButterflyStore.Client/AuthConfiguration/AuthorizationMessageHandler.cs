using Blazored.LocalStorage;
using System.Net.Http.Headers;

public class AuthorizationMessageHandler : DelegatingHandler
{
    private readonly ILocalStorageService _localStorage;

    public AuthorizationMessageHandler(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        //Check if there's a key for the secruity token in the local storage.
        if(await _localStorage.ContainKeyAsync("access_token"))
        {
            //Get the token from the local storage , and then attach it to the request header.
            var token = await _localStorage.GetItemAsStringAsync("access_token");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
        Console.WriteLine("Authoriazation Message Handler Has Been Called.");

        return await base.SendAsync(request, cancellationToken);
    }
}
