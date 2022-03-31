using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMSApplication.Models;

namespace WMSApplication.Contracts
{
    public interface IAuthUserRepository
    {
        Task<IEnumerable<AuthUser>> FindAllAsync();
    }
}
