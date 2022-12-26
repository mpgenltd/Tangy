using System.Net.Http.Headers;
using System.Text;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using Tangy_Models;

namespace TangyWeb_Client.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _client;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authStateProvider;

        public AuthenticationService(HttpClient client, ILocalStorageService localStorage, AuthenticationStateProvider authStateProvider)
        {
            _client = client;
            _authStateProvider = authStateProvider;
            _localStorage = localStorage;
        }

        public async Task<SignInResponseDto> Login(SignInRequestDto signInRequest)
        {
            var content = JsonConvert.SerializeObject(signInRequest);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("api/account/signin", bodyContent);
            var contentTemp = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<SignInResponseDto>(contentTemp);

            if (response.IsSuccessStatusCode)
            {
                await _localStorage.SetItemAsync("Local_Token", result.Token);
                await _localStorage.SetItemAsync("Local_UserDetails", result.UserDto);
                ((AuthStateProvider)_authStateProvider).NotifyUserLoggedIn(result.Token);
                _client.DefaultRequestHeaders.Authorization= new AuthenticationHeaderValue("bearer", result.Token);
                return new SignInResponseDto() { IsAuthSuccessful = true };
            }
            else
            {
                return result;
            }
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("Local_Token");
            await _localStorage.RemoveItemAsync("Local_UserDetails");

            ((AuthStateProvider)_authStateProvider).NotifyUserLogout();

            _client.DefaultRequestHeaders.Authorization= null;
        }

        public async Task<SignUpResponseDto> RegisterUser(SignUpRequestDto signUpRequest)
        {
            var content = JsonConvert.SerializeObject(signUpRequest);
            var bodyContent = new StringContent(content, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("api/account/signup", bodyContent);
            var contentTemp = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<SignUpResponseDto>(contentTemp);

            if (response.IsSuccessStatusCode)
            {
                return new SignUpResponseDto { IsRegisterationSuccessful = true };
            }
            else
            {
                return new SignUpResponseDto { IsRegisterationSuccessful = false, Errors=result.Errors };
            }
        }
    }
}
