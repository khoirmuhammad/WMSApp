using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMSApplication.CustomModels;
using WMSApplication.Models;

namespace WMSApplication.Contracts
{
    public interface IProductCategoryRepository
    {
        Task<IEnumerable<ProductCategory>> FindAllAsync();
        Task<IEnumerable<ProductCategory>> FindAllAsync(string sortPropert, SortOrder sortOrder, string searchText = "");
        Task<ProductCategory> FindAsyncById(string code);
        void Create(ProductCategory productCategory);
        void Update(ProductCategory productCategory);
        void Delete(ProductCategory productCategory);
    }
}
