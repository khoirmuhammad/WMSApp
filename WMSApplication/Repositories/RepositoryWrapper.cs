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

        private IUnitRepository _unitRepository;
        private IProductCategoryRepository _producCategory;

        public RepositoryWrapper(ApplicationContext applicationContext)
        {
            this._applicationContext = applicationContext;
        }

        public IUnitRepository Unit
        {
            get
            {
                if (_unitRepository == null)
                    _unitRepository = new UnitRepository(_applicationContext);

                return _unitRepository;
            }
        }

        public IProductCategoryRepository ProductCategory
        {
            get
            {
                if (_producCategory == null)
                    _producCategory = new ProductCategoryRepository(_applicationContext);

                return _producCategory;
            }
        }

        public async Task SaveAsync()
        {
            await _applicationContext.SaveChangesAsync();
        }
    }
}
