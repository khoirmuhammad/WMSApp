using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMSApplication.CustomModels;
using WMSApplication.Models;

namespace WMSApplication.Contracts
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> FindAllAsync();
        Task<IEnumerable<Product>> FindAllAsync(string sortProperty, SortOrder sortOrder, string searchText = "");
        Task<Product> FindAsyncById(string code);
        void Create(Product product);
        void Update(Product product);
        void Delete(Product product);
    }
}
