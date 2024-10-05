using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class EmployeeRefreshToken
    {
        public int EmployeeId { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryDate { get; set; }
        public virtual Account Account { get; set; }
    }
}
