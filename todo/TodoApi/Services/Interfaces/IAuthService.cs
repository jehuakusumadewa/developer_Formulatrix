using TodoApi.DTOs.Requests;
using TodoApi.DTOs.Responses;
using TodoApi.Helpers;

namespace TodoApi.Services.Interfaces
{
    public interface IAuthService
    {
        Task<Result<UserResponse>> RegisterAsync(RegisterRequest request);
        Task<Result<string>> LoginAsync(LoginRequest request);
    }
}