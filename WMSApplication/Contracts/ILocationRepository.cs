using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMSApplication.CustomModels;
using WMSApplication.Models;

namespace WMSApplication.Contracts
{
    public interface ILocationRepository
    {
        Task<IEnumerable<Location>> FindAllAsync();
        Task<IEnumerable<Location>> FindAllAsync(string sortProperty, SortOrder sortOrder, string searchText = "");
        Task<Location> FindAsyncById(string code);
        void Create(Location product);
        void Update(Location product);
        void Delete(Location product);
    }
}
