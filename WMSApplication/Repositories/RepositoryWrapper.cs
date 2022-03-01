using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMSApplication.Contracts;
using WMSApplication.Data;

namespace WMSApplication.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private ApplicationContext _applicationContext;

        private IUnitRepository _unit;
        private IProductCategoryRepository _productCategory;
        private IProductRepository _product;
        private ILocationRepository _location;

        public RepositoryWrapper(ApplicationContext applicationContext)
        {
            this._applicationContext = applicationContext;
        }

        public IUnitRepository Unit
        {
            get
            {
                if (_unit == null)
                    _unit = new UnitRepository(_applicationContext);

                return _unit;
            }
        }

        public IProductCategoryRepository ProductCategory
        {
            get
            {
                if (_productCategory == null)
                    _productCategory = new ProductCategoryRepository(_applicationContext);

                return _productCategory;
            }
        }

        public IProductRepository Product
        {
            get
            {
                if (_product == null)
                    _product = new ProductRepository(_applicationContext);

                return _product;
            }
        }

        public ILocationRepository Location
        {
            get
            {
                if (_location == null)
                    _location = new LocationRepository(_applicationContext);

                return _location;
            }
        }

        public async Task SaveAsync()
        {
            await _applicationContext.SaveChangesAsync();
        }
    }
}
