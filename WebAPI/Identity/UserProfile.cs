using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPI.Identity
{
    public class UserProfile : IdentityUser
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Login { get; set; }
        public string Role { get; set; }
    }
}
