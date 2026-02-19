using Application.Serices.Abstractions;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ServiceManager(
        UserManager<User> userManager,
        IEmailService emailService ,
        IJwtTokenService jwtTokenService,
        RoleManager<IdentityRole<int>> roleManager) : IServiceManager
    {
        public IAuthService AuthService { get; } = new AuthService(userManager,emailService,jwtTokenService,roleManager);
    }
}
