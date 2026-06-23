using OnlineExam.Common;
using OnlineExam.DTOs.AuthDTOs;
using OnlineExam.Entity;

namespace OnlineExam.ServiceInterfaces
{
    public interface IUserService
    {
        Task<Result> RegisterAsync(RegisterDTO dto);
        Task<Result<TokenResponse>> LoginAsync(LoginDTO dto);
        Task<Result<TokenResponse>> RefreshToken(string refreshToken);
    }
}
