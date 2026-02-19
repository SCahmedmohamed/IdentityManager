using Application.DTOs.Auth;
using Application.Serices.Abstractions;
using Domain.Entities;
using Domain.Exceptions;
using Infrastructure.DbContexts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AuthService(UserManager<User> _userManager , IEmailService _emailService , IJwtTokenService _jwtTokenService , RoleManager<IdentityRole<int>> _roleManager ) : IAuthService
    {
        public async Task<bool> AddUserToRoleAsync(int userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user is null)
                throw new BadRequestException("User not found");
            if(await _userManager.IsInRoleAsync(user, role))
                throw new BadRequestException("User is already in this role");
            var res = await _userManager.AddToRoleAsync(user, role);
            if (!res.Succeeded)
                throw new BadRequestException(string.Join(", ", res.Errors.Select(e => e.Description)));
            return true;
        }
        public async Task<bool> AssignRolesToUserAsync(int userId, List<string> roles)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user is null)
                throw new BadRequestException("User not found");
            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                    throw new BadRequestException($"Role '{role}' does not exist");
            }
            var currentRoles = await _userManager.GetRolesAsync(user);
            var rolesToAdd = roles.Except(currentRoles).ToList();
            var rolesToRemove = currentRoles.Except(roles).ToList();
            var resAdd = await _userManager.AddToRolesAsync(user, rolesToAdd);
            if (!resAdd.Succeeded)
                throw new BadRequestException(string.Join(", ", resAdd.Errors.Select(e => e.Description)));
            var resRemove = await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
            if (!resRemove.Succeeded)
                throw new BadRequestException(string.Join(", ", resRemove.Errors.Select(e => e.Description)));
            return true;
        }
        public async Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user is null)
                throw new BadRequestException("User not found");
            var res = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            if (!res.Succeeded)
                throw new BadRequestException(string.Join(", ", res.Errors.Select(e => e.Description)));
            return true;
        }
        public async Task<bool> ConfirmEmailAsync(string email, string otp)
        {
            var user =await _userManager.FindByEmailAsync(email);
            if (user is null)
                throw new BadRequestException("Invalid email");
            if (user.EmailConfirmationCode != otp || user.EmailConfirmationCodeExpiry is null || user.EmailConfirmationCodeExpiry < DateTime.UtcNow)
                throw new BadRequestException("Invalid email or OTP");
            if (user.EmailConfirmed == true)
                throw new BadRequestException("Email is already confirmed");
            user.EmailConfirmed = true;
            user.EmailConfirmationCode = null;
            user.EmailConfirmationCodeExpiry = null;
            var res = await _userManager.UpdateAsync(user);
            if (!res.Succeeded)
                throw new BadRequestException(string.Join(", ", res.Errors.Select(e => e.Description)));
            return true;
        }
        public async Task<AuthResponse> ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                throw new BadRequestException("Invalid email");
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var otp = new Random().Next(100000, 999999).ToString();
            user.EmailConfirmationCode = otp;
            user.EmailConfirmationCodeExpiry = DateTime.UtcNow.AddMinutes(15);
            await _userManager.UpdateAsync(user);
            await _emailService.SendEmailAsync(user.Email, "Password Reset", $"Your OTP for password reset is: {otp}. It will expire in 15 minutes.");
            return new AuthResponse
            {
                IsSuccess = true,
                Message = "Password reset OTP sent to email",
                EmailConfirmationToken = token,
                UserEmail = user.Email,
                UserName = user.UserName
            };

        }
        public async Task<List<string>> GetUserRolesAsync(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user is null)
                throw new BadRequestException("User not found");
            var roles = await _userManager.GetRolesAsync(user);
            return roles.ToList();

        }
        public async Task<AuthResponse> LoginAsync(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            var password = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (user is null || password == false)
                return new AuthResponse
                {
                    IsSuccess = false,
                    Message = "Invalid email or password"
                };
            if(!await _userManager.IsEmailConfirmedAsync(user))
                return new AuthResponse
                {
                    IsSuccess = false,
                    Message = "Email is not confirmed"
                };
            var token = await _jwtTokenService.GenerateTokenAsync(user);
            return new AuthResponse
            {
                IsSuccess = true,
                Message = "Login successful",
                EmailConfirmationToken = token,
                UserEmail = user.Email,
                UserName = user.UserName
            };
        }
        public async Task<AuthResponse> RegisterAsync(RegisterDto dto)
        {
            var emailvalid =  await _userManager.FindByEmailAsync(dto.Email);
            if(emailvalid is not null)
                return new AuthResponse
                {
                    IsSuccess = false,
                    Message = "Email is already in use"
                };
            if (dto.PasswordRepeat != dto.Password)
                return new AuthResponse
                {
                    IsSuccess = false,
                    Message = "Passwords do not match"
                };

            var user = new User
            {
                UserName = dto.UserName,
                Email = dto.Email,
                DisplayName = dto.DisplayName,
                DOB = dto.DOB,
                PhoneNumber = dto.PhoneNumber,
                EmailConfirmed = false
            };

            var res = await _userManager.CreateAsync(user, dto.Password);

            if (!res.Succeeded)
            {
                return new AuthResponse
                {
                    IsSuccess = false,
                    Message = string.Join(", ", res.Errors.Select(e => e.Description))
                };
            }
            var token = await _jwtTokenService.GenerateTokenAsync(user);


            var otp = new Random().Next(100000, 999999).ToString();
            user.EmailConfirmationCode = otp;
            user.EmailConfirmationCodeExpiry = DateTime.UtcNow.AddMinutes(15);
            await _userManager.UpdateAsync(user);

            await _emailService.SendEmailAsync(user.Email, "Email Confirmation", $"Your OTP for email confirmation is: {otp}. It will expire in 15 minutes.");


            return new AuthResponse
            {
                IsSuccess = true,
                Message = "Registration successful. Please check your email to confirm your account.",
                EmailConfirmationToken = token,
                UserEmail = dto.Email,
                UserName = dto.UserName,
            };

        }
        public async Task<bool> RemoveUserFromRoleAsync(int userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user is null)
                throw new BadRequestException("User not found");
            if (!await _userManager.IsInRoleAsync(user, role))
                throw new BadRequestException("User is not in this role");
            var res = await _userManager.RemoveFromRoleAsync(user, role);
            if (!res.Succeeded)
                throw new BadRequestException(string.Join(", ", res.Errors.Select(e => e.Description)));
            return true;

        }
        public async Task<bool> ResetPasswordRequestAsync(ResetPasswordRequestDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user is null)
                throw new BadRequestException("Invalid email");
            var otp = new Random().Next(100000, 999999).ToString();
            user.EmailConfirmationCode = otp;
            user.EmailConfirmationCodeExpiry = DateTime.UtcNow.AddMinutes(15);
            await _userManager.UpdateAsync(user);
            await _emailService.SendEmailAsync(user.Email, "Password Reset", $"Your OTP for password reset is: {otp}. It will expire in 15 minutes.");
            return true;
        }
        public async Task<bool> ConfrimResetPasswordAsync(ResetPasswordDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user is null)
                throw new BadRequestException("Invalid email");
            if (user.EmailConfirmationCode != dto.Otp || user.EmailConfirmationCodeExpiry is null || user.EmailConfirmationCodeExpiry < DateTime.UtcNow)
                throw new BadRequestException("Invalid email or OTP");
            if (dto.NewPassword != dto.ConfirmPassword)
                throw new BadRequestException("Passwords do not match");
            var res = await _userManager.ResetPasswordAsync(user, await _userManager.GeneratePasswordResetTokenAsync(user), dto.NewPassword);
            if (!res.Succeeded)
                throw new BadRequestException(string.Join(", ", res.Errors.Select(e => e.Description)));
            user.EmailConfirmationCode = null;
            user.EmailConfirmationCodeExpiry = null;
            await _userManager.UpdateAsync(user);
            return true;
        }


    }
}
