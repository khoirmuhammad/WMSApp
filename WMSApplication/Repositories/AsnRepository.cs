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
    public class AsnRepository : RepositoryBase<Asn>, IAsnRepository
    {
        public AsnRepository(ApplicationContext applicationContext) : base(applicationContext)
        {

        }

        public async Task<IEnumerable<Asn>> FindAllAsync()
        {
            return await base.FindAll().ToListAsync();
        }

        public async Task<IEnumerable<Asn>> FindAllAsync(string sortProperty, SortOrder sortOrder, string searchText = "")
        {
            IEnumerable<Asn> asns;

            if (string.IsNullOrEmpty(searchText))
            {
                asns = await base.FindAll().ToListAsync();
            }
            else
            {
                int searchTextToInt = Convert.ToInt32(searchText);

                asns = await base.FindAll().Where(x => x.Code.Equals(searchText)).ToListAsync();
            }

            asns = PerformSort(asns, sortProperty, sortOrder);

            return asns;
        }

        public async Task<Asn> FindAsyncById(string code)
        {
            return await base.FindByCondition(x => x.Code.Equals(code)).Include(ad => ad.AsnDetails.OrderBy(y => y.LineNumber)).FirstOrDefaultAsync();
        }

        public void Create(Asn asn)
        {
            base.Create(asn);
        }

        public void Update(Asn asn)
        {
            base.Update(asn);
        }

        public void Delete(Asn asn)
        {
            base.Delete(asn);
        }

        #region Custom Method

        private IEnumerable<Asn> DynamicOrderQuery(IEnumerable<Asn> source, string propertyName, SortOrder sortOrder)
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

        private IEnumerable<Asn> DynamicWhereQuery(IEnumerable<Asn> source, string propertyName, string propertyValue)
        {
            return source.Where(src => { return src.GetType().GetProperty(propertyName).GetValue(src, null).ToString().StartsWith(propertyValue); });
        }

        #endregion

        #region Support Method
        private IEnumerable<Asn> PerformSort(IEnumerable<Asn> source, string propertyName, SortOrder sortOrder)
        {
            return DynamicOrderQuery(source, propertyName, sortOrder);
        }
        #endregion
    }
}
