using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMSApplication.Contracts;
using WMSApplication.Data;
using WMSApplication.Models;
using WMSApplication.CustomModels;

namespace WMSApplication.Repositories
{
    public class UnitRepository : RepositoryBase<Unit>, IUnitRepository
    {
        public UnitRepository(ApplicationContext applicationContext) : base(applicationContext)
        {

        }
        public async Task<IEnumerable<Unit>> FindAllAsync()
        {
            return await base.FindAll().ToListAsync();
        }

        public async Task<IEnumerable<Unit>> FindAllAsync(string sortProperty, SortOrder sortOrder, string searchText = "")
        {
            IEnumerable<Unit> units;

            if (string.IsNullOrEmpty(searchText))
            {
                units = await base.FindAll().ToListAsync();
            }
            else
            {
                units = await base.FindAll().Where(x => x.Name.Equals(searchText) || x.Description.Equals(searchText)).ToListAsync();
            }

            units = PerformSort(units, sortProperty, sortOrder);

            return units;
        }

        public async Task<Unit> FindAsyncById(string code)
        {
            return await base.FindByCondition(x => x.Code.Equals(code)).FirstOrDefaultAsync();
        }

        public void Create(Unit unit)
        {
            base.Create(unit);
        }

        public void Update(Unit unit)
        {
            base.Update(unit);
        }

        public void Delete(Unit unit)
        {
            base.Delete(unit);
        }

        #region Custom Method

        private IEnumerable<Unit> DynamicOrderQuery(IEnumerable<Unit> source, string propertyName, SortOrder sortOrder)
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

        private IEnumerable<Unit> DynamicWhereQuery(IEnumerable<Unit> source, string propertyName, string propertyValue)
        {
            return source.Where(src => { return src.GetType().GetProperty(propertyName).GetValue(src, null).ToString().StartsWith(propertyValue); });
        }

        #endregion

        #region Support Method
        private IEnumerable<Unit> PerformSort(IEnumerable<Unit> source, string propertyName, SortOrder sortOrder)
        {
            return DynamicOrderQuery(source, propertyName, sortOrder);
        }
        #endregion
    }
}
