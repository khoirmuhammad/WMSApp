using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WMSApplication.Models
{
    public class AuthUserRole
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        public AuthUser AuthUser { get; set; }

        public Guid RoleId { get; set; }
        public AuthRole AuthRole { get; set; }
    }
}
