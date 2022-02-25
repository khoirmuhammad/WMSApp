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
    public class ProductRepository : RepositoryBase<Product>, IProductRepository
    {
        public ProductRepository(ApplicationContext applicationContext) : base(applicationContext)
        {

        }
        public async Task<IEnumerable<Product>> FindAllAsync()
        {
            return await base.FindAll().ToListAsync();
        }

        public async Task<IEnumerable<Product>> FindAllAsync(string sortProperty, SortOrder sortOrder, string searchText = "")
        {
            IEnumerable<Product> products;

            if (string.IsNullOrEmpty(searchText))
            {
                products = await base.FindAll().ToListAsync();
            }
            else
            {
                products = await base.FindAll().Where(x => x.Name.Equals(searchText) || x.Code.Equals(searchText) || x.CategoryCode.Equals(searchText) || x.UnitCode.Equals(searchText) || x.AllocationType.Equals(searchText) || x.WholeUnit.Equals(searchText) || x.Weight.Equals(searchText) || x.LooseQty.Equals(Convert.ToInt32(searchText)) || x.WholeQty.Equals(Convert.ToInt32(searchText)) || x.LoosePrice.Equals(Convert.ToInt32(searchText)) || x.WholePrice.Equals(Convert.ToInt32(searchText))).ToListAsync();
            }

            products = PerformSort(products, sortProperty, sortOrder);

            return products;
        }
        public async Task<Product> FindAsyncById(string code)
        {
            return await base.FindByCondition(x => x.Code.Equals(code)).FirstOrDefaultAsync();
        }

        public void Create(Product product)
        {
            base.Create(product);
        }

        public void Update(Product product)
        {
            base.Update(product);
        }

        public void Delete(Product product)
        {
            base.Delete(product);
        }


        #region Custom Method

        private IEnumerable<Product> DynamicOrderQuery(IEnumerable<Product> source, string propertyName, SortOrder sortOrder)
        {
            if (sortOrder == SortOrder.Ascending)
            {
                return source.OrderBy(src => { return src.GetType().GetProperty(propertyName).GetValue(src); });
            }
            else
            {
                return source.OrderByDescending(src => { return src.GetType().GetProperty(propertyName).GetValue(src); });
            }
        }

        private IEnumerable<Product> DynamicWhereQuery(IEnumerable<Product> source, string propertyName, string propertyValue)
        {
            return source.Where(src => { return src.GetType().GetProperty(propertyName).GetValue(src, null).ToString().StartsWith(propertyValue); });
        }

        #endregion

        #region Support Method
        private IEnumerable<Product> PerformSort(IEnumerable<Product> source, string propertyName, SortOrder sortOrder)
        {
            return DynamicOrderQuery(source, propertyName, sortOrder);
        }
        #endregion
    }
}
