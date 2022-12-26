using Tangy_Models;

namespace TangyWeb_Client.Services
{
    public interface IAuthenticationService
    {
        Task<SignUpResponseDto> RegisterUser(SignUpRequestDto signUpRequestDto);
        Task<SignInResponseDto> Login(SignInRequestDto signInRequestDto);
        Task Logout();
    }
}
