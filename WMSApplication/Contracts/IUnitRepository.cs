using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMSApplication.Models;
using WMSApplication.CustomModels;

namespace WMSApplication.Contracts
{
    public interface IUnitRepository
    {
        Task<IEnumerable<Unit>> FindAllAsync();
        Task<IEnumerable<Unit>> FindAllAsync(string sortProperty, SortOrder sortOrder, string searchText = "");
        Task<Unit> FindAsyncById(string code);
        void Create(Unit unit);
        void Update(Unit unit);
        void Delete(Unit unit);
    }
}
