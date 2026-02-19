using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User : IdentityUser<int>
    {
        public string DisplayName { get; set; }
        public DateTime DOB { get; set; }
        public string? EmailConfirmationCode { get; set; }
        public DateTime? EmailConfirmationCodeExpiry { get; set; }
    }
}
