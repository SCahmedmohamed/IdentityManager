using Application.DTOs.Auth;
using Domain.Entities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Serices.Abstractions
{
    public interface IJwtTokenService
    {
        Task<string> GenerateTokenAsync(User user);
    }
}
