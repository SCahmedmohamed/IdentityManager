using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Auth
{
    public class AuthResponse
    {
        public bool IsSuccess { get; set; }
        public string? EmailConfirmationToken { get; set; }
        public string? Message { get; set; }
        public DateTime? Expiration { get; set; }
        public string? UserName { get; set; }    
        public string? UserEmail { get; set; }
    }
}
