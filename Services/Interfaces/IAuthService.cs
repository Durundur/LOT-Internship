using LOT_TASK.Dtos;

namespace LOT_TASK.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> Login(AuthRequestDto credentials);
        Task<AuthResponseDto> RefreshToken(RefreshTokenRequest jwt);
        Task<AuthResponseDto> Register(AuthRequestDto credentials);
    }
}