using Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Serices.Abstractions
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterAsync(RegisterDto dto);
        Task<AuthResponse> LoginAsync(LoginDto dto);
        Task<bool> ConfirmEmailAsync(string email , string otp);
        Task<AuthResponse> ForgotPasswordAsync(string email);
        Task<bool> ResetPasswordRequestAsync(ResetPasswordRequestDto dto);
        Task<bool> ConfrimResetPasswordAsync(ResetPasswordDto dto);
        Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
        Task<List<string>> GetUserRolesAsync(int userId);
        Task<bool> AddUserToRoleAsync(int userId, string role);
        Task<bool> AssignRolesToUserAsync(int userId, List<string> roles);
        Task<bool> RemoveUserFromRoleAsync(int userId, string role);
    }
}
