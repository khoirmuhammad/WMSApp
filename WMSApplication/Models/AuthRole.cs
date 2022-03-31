using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WMSApplication.Models
{
    public class AuthRole
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public IList<AuthUserRole> AuthUserRole { get; set; }
    }
}
