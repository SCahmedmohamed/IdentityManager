using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Auth
{
    public class RegisterDto
    {
        public string DisplayName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PasswordRepeat { get; set; }
        [Phone]  
        public string PhoneNumber { get; set; }
        public DateTime DOB { get; set; }
    }
}
