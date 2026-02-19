using Application.DTOs.Auth;
using Application.Serices.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IServiceManager _serviceManager) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var result = await _serviceManager.AuthService.RegisterAsync(dto);
            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var result = await _serviceManager.AuthService.LoginAsync(dto);
            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result);
        }
        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromBody] string email, string otp)
        {
            var result = await _serviceManager.AuthService.ConfirmEmailAsync(email, otp);
            if (!result)
                return BadRequest("Invalid OTP or email.");

            return Ok("Email confirmed successfully.");
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] string email)
        {
            var result = await _serviceManager.AuthService.ForgotPasswordAsync(email);
            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result);
        }
        [HttpPost("reset-password-request")]
        public async Task<IActionResult> ResetPasswordRequest([FromBody] ResetPasswordRequestDto dto)
        {
            var result = await _serviceManager.AuthService.ResetPasswordRequestAsync(dto);
            if (!result)
                return BadRequest("Failed to send reset password email.");

            return Ok("Reset password email sent successfully.");
        }
        [HttpPost("confirm-reset-password")]
        public async Task<IActionResult> ConfirmResetPassword([FromBody] ResetPasswordDto dto)
        {
            var result = await _serviceManager.AuthService.ConfrimResetPasswordAsync(dto);
            if (!result)
                return BadRequest("Failed to reset password. Please check your OTP and email.");

            return Ok("Password reset successfully.");
        }
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(int userId, [FromBody] ChangePasswordDto dto)
        {
            var result = await _serviceManager.AuthService.ChangePasswordAsync(userId, dto.CurrentPassword, dto.NewPassword);
            if (!result)
                return BadRequest("Failed to change password. Please check your current password.");

            return Ok("Password changed successfully.");
        }
        [HttpGet("user-roles/{userId}")]
        public async Task<IActionResult> GetUserRoles(int userId)
        {
            var roles = await _serviceManager.AuthService.GetUserRolesAsync(userId);
            return Ok(roles);
        }
        [HttpPost("add-user-to-role")]
        public async Task<IActionResult> AddUserToRole(int userId, [FromBody] string role)
        {
            var result = await _serviceManager.AuthService.AddUserToRoleAsync(userId, role);
            if (!result)
                return BadRequest("Failed to add user to role.");

            return Ok("User added to role successfully.");
        }
        [HttpPost("assign-roles-to-user")]
        public async Task<IActionResult> AssignRolesToUser(int userId, [FromBody] List<string> roles)
        {
            var result = await _serviceManager.AuthService.AssignRolesToUserAsync(userId, roles);
            if (!result)
                return BadRequest("Failed to assign roles to user.");

            return Ok("Roles assigned to user successfully.");
        }
        [HttpPost("remove-user-from-role")]
        public async Task<IActionResult> RemoveUserFromRole(int userId, [FromBody] string role)
        {
            var result = await _serviceManager.AuthService.RemoveUserFromRoleAsync(userId, role);
            if (!result)
                return BadRequest("Failed to remove user from role.");

            return Ok("User removed from role successfully.");
        }
        
    }
}
