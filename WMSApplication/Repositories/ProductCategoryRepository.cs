using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMSApplication.Models;
using WMSApplication.Contracts;
using WMSApplication.CustomModels;
using WMSApplication.Data;
using Microsoft.EntityFrameworkCore;

namespace WMSApplication.Repositories
{
    public class ProductCategoryRepository : RepositoryBase<ProductCategory>, IProductCategoryRepository
    {
        public ProductCategoryRepository(ApplicationContext applicationContext) : base(applicationContext)
        {

        }
        public async Task<IEnumerable<ProductCategory>> FindAllAsync()
        {
            return await base.FindAll().ToListAsync();
        }

        public async Task<IEnumerable<ProductCategory>> FindAllAsync(string sortProperty, SortOrder sortOrder, string searchText = "")
        {
            IEnumerable<ProductCategory> productCategories;

            if (string.IsNullOrEmpty(searchText))
            {
                productCategories = await base.FindAll().ToListAsync();
            }
            else
            {
                productCategories = await base.FindAll().Where(x => x.Code.Equals(searchText) || x.Name.Equals(searchText) || x.Description.Equals(searchText)).ToListAsync();
            }

            productCategories = PerformSort(productCategories, sortProperty, sortOrder);

            return productCategories;
        }

        public async Task<ProductCategory> FindAsyncById(string code)
        {
            return await base.FindByCondition(x => x.Code.Equals(code)).FirstOrDefaultAsync();
        }

        public void Create(ProductCategory productCategory)
        {
            base.Create(productCategory);
        }

        public void Update(ProductCategory productCategory)
        {
            base.Update(productCategory);
        }

        public void Delete(ProductCategory productCategory)
        {
            base.Delete(productCategory);
        }


        #region Custom Method

        private IEnumerable<ProductCategory> DynamicOrderQuery(IEnumerable<ProductCategory> source, string propertyName, SortOrder sortOrder)
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

        private IEnumerable<ProductCategory> DynamicWhereQuery(IEnumerable<ProductCategory> source, string propertyName, string propertyValue)
        {
            return source.Where(src => { return src.GetType().GetProperty(propertyName).GetValue(src, null).ToString().StartsWith(propertyValue); });
        }

        #endregion

        #region Support Method
        private IEnumerable<ProductCategory> PerformSort(IEnumerable<ProductCategory> source, string propertyName, SortOrder sortOrder)
        {
            return DynamicOrderQuery(source, propertyName, sortOrder);
        }
        #endregion
    }
}
