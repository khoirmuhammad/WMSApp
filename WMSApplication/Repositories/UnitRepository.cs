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

        public async Task<IEnumerable<Unit>> FindAllAsync(string sortProperty, SortOrder sortOrder)
        {
            IEnumerable<Unit> units = await base.FindAll().ToListAsync();

            return DynamicOrderQuery(units, sortProperty, sortOrder);
        }

        public async Task<Unit> FindAsyncById(int id)
        {
            return await base.FindByCondition(x => x.Id.Equals(id)).FirstOrDefaultAsync();
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

        public IEnumerable<Unit> DynamicOrderQuery(IEnumerable<Unit> source, string propertyName, SortOrder sortOrder)
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

        public IEnumerable<Unit> DynamicWhereQuery(IEnumerable<Unit> source, string propertyName, string propertyValue)
        {
            return source.Where(src => { return src.GetType().GetProperty(propertyName).GetValue(src, null).ToString().StartsWith(propertyValue); });
        }

        #endregion
    }
}
