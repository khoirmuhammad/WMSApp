using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMSApplication.Contracts;
using WMSApplication.CustomModels;
using WMSApplication.Data;
using WMSApplication.Models;

namespace WMSApplication.Repositories
{
    public class AuthUserRepository : RepositoryBase<AuthUser>, IAuthUserRepository
    {
        public AuthUserRepository(ApplicationContext applicationContext) : base(applicationContext)
        {

        }

        public async Task<IEnumerable<AuthUser>> FindAllAsync()
        {
            return await base.FindAll().ToListAsync();
        }

    }
}
