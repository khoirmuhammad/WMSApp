using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMSApplication.Contracts;
using WMSApplication.Models;
using WMSApplication.Data;
using WMSApplication.CustomModels;
using Microsoft.EntityFrameworkCore;

namespace WMSApplication.Repositories
{
    public class AsnDetailRepository : RepositoryBase<AsnDetail>, IAsnDetailRepository
    {
        public AsnDetailRepository(ApplicationContext applicationContext) : base(applicationContext)
        {

        }

        public async Task<IEnumerable<AsnDetail>> FindAllAsync()
        {
            return await base.FindAll().ToListAsync();
        }

        public async Task<IEnumerable<AsnDetail>> FindAllAsync(string sortProperty, SortOrder sortOrder, string searchText = "")
        {
            IEnumerable<AsnDetail> asnDetails;

            if (string.IsNullOrEmpty(searchText))
            {
                asnDetails = await base.FindAll().ToListAsync();
            }
            else
            {
                int searchTextToInt = Convert.ToInt32(searchText);

                asnDetails = await base.FindAll().ToListAsync();
            }

            asnDetails = PerformSort(asnDetails, sortProperty, sortOrder);

            return asnDetails;
        }

        public async Task<IEnumerable<AsnDetail>> FindAsyncByAsnCode(string asnCode)
        {
            return await base.FindByCondition(x => x.AsnCode.Equals(asnCode)).ToListAsync();
        }

        public async Task<AsnDetail> FindAsyncById(int id)
        {
            return await base.FindByCondition(x => x.Id.Equals(id)).FirstOrDefaultAsync();
        }

        public void Create(AsnDetail asndetail)
        {
            base.Create(asndetail);
        }

        public void Update(AsnDetail asndetail)
        {
            base.Update(asndetail);
        }

        public void Delete(AsnDetail asndetail)
        {
            base.Delete(asndetail);
        }

        public void Delete(List<AsnDetail> asnDetails)
        {
            foreach(var item in asnDetails)
            {
                base.Delete(item);
            }
        }

        #region Custom Method

        private IEnumerable<AsnDetail> DynamicOrderQuery(IEnumerable<AsnDetail> source, string propertyName, SortOrder sortOrder)
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

        private IEnumerable<AsnDetail> DynamicWhereQuery(IEnumerable<AsnDetail> source, string propertyName, string propertyValue)
        {
            return source.Where(src => { return src.GetType().GetProperty(propertyName).GetValue(src, null).ToString().StartsWith(propertyValue); });
        }

        #endregion

        #region Support Method
        private IEnumerable<AsnDetail> PerformSort(IEnumerable<AsnDetail> source, string propertyName, SortOrder sortOrder)
        {
            return DynamicOrderQuery(source, propertyName, sortOrder);
        }
        #endregion
    }
}
