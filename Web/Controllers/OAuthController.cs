using Application.Serices.Abstractions;
using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OAuthController(UserManager<User> _userManager , IJwtTokenService _jwtTokenService) : ControllerBase
    {
        [HttpGet("google-login")]
        public IActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleResponse")
            };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }
        [HttpGet("google-response")]
        public async Task<IActionResult> GoogleResponse()
        {
            var result =await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);
            var email = result.Principal?.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;
            var name = result.Principal?.FindFirst(c => c.Type == ClaimTypes.Name)?.Value;
            var user = await _userManager.FindByEmailAsync(email);
            if(user == null)
            {
                user = new User
                {
                    Email = email,
                    DisplayName = name,
                    UserName = email
                };
                await _userManager.CreateAsync(user);
            }
            var token = await _jwtTokenService.GenerateTokenAsync(user);
            return Ok(new { Token = token });
        }

        [HttpGet("github-login")]
        public IActionResult GitHubLogin()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(GitHubResponse))
            };
            return Challenge(properties, "GitHub");
        }

        [HttpGet("github-response")]
        public async Task<IActionResult> GitHubResponse()
        {
            var result = await HttpContext.AuthenticateAsync("GitHub");

            if (!result.Succeeded)
                return BadRequest("External authentication failed");

            var email = result.Principal?.FindFirst(ClaimTypes.Email)?.Value;
            var name = result.Principal?.FindFirst(ClaimTypes.Name)?.Value;

            if (email is null)
                return BadRequest("Email not returned from GitHub");

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new User
                {
                    Email = email,
                    DisplayName = name,
                    UserName = email
                };
                await _userManager.CreateAsync(user);
            }

            var token = await _jwtTokenService.GenerateTokenAsync(user);

            return Ok(new { Token = token });
        }

    }
}
