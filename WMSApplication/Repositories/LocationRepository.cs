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
    public class LocationRepository: RepositoryBase<Location>, ILocationRepository
    {
        public LocationRepository(ApplicationContext applicationContext) : base(applicationContext)
        {

        }

        public async Task<IEnumerable<Location>> FindAllAsync()
        {
            return await base.FindAll().ToListAsync();
        }

        public async Task<IEnumerable<Location>> FindAllAsync(string sortProperty, SortOrder sortOrder, string searchText = "")
        {
            IEnumerable<Location> locations;

            if (string.IsNullOrEmpty(searchText))
            {
                locations = await base.FindAll().ToListAsync();
            }
            else
            {
                int searchTextToInt = Convert.ToInt32(searchText);

                locations = await base.FindAll().Where(x => x.Code.Equals(searchText) || x.Floor.Equals(searchTextToInt) || x.RackAisle.Equals(searchText) || x.Level.Equals(searchTextToInt) || x.Pos.Equals(searchTextToInt) || x.Type.Equals(searchText) || x.MaximumPallet.Equals(searchTextToInt)).ToListAsync();
            }

            locations = PerformSort(locations, sortProperty, sortOrder);

            return locations;
        }

        public async Task<Location> FindAsyncById(string code)
        {
            return await base.FindByCondition(x => x.Code.Equals(code)).FirstOrDefaultAsync();
        }

        public void Create(Location location)
        {
            base.Create(location);
        }

        public void Update(Location location)
        {
            base.Update(location);
        }

        public void Delete(Location location)
        {
            base.Delete(location);
        }


        #region Custom Method

        private IEnumerable<Location> DynamicOrderQuery(IEnumerable<Location> source, string propertyName, SortOrder sortOrder)
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

        private IEnumerable<Location> DynamicWhereQuery(IEnumerable<Location> source, string propertyName, string propertyValue)
        {
            return source.Where(src => { return src.GetType().GetProperty(propertyName).GetValue(src, null).ToString().StartsWith(propertyValue); });
        }

        #endregion

        #region Support Method
        private IEnumerable<Location> PerformSort(IEnumerable<Location> source, string propertyName, SortOrder sortOrder)
        {
            return DynamicOrderQuery(source, propertyName, sortOrder);
        }
        #endregion

    }
}
