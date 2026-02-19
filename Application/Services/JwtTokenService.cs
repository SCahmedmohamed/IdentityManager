using Application.DTOs.Auth;
using Application.Serices.Abstractions;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class JwtTokenService(UserManager<User> _userManager , JwtSettings _settings) : IJwtTokenService
    {

        public async Task<string> GenerateTokenAsync(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim("id", user.Id.ToString()),
                new Claim("email", user.Email),
                new Claim("name",user.DisplayName),
            };
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = roles.Select(r => new Claim(ClaimTypes.Role, r));
             
            var token = new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims.Concat(roleClaims),
                expires: DateTime.UtcNow.AddMinutes(_settings.ExpirationInMinutes),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);

        }
    }
}
