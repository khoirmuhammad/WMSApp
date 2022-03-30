using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMSApplication.CustomModels;
using WMSApplication.Models;

namespace WMSApplication.Contracts
{
    public interface IAsnDetailRepository
    {
        Task<IEnumerable<AsnDetail>> FindAllAsync();
        Task<IEnumerable<AsnDetail>> FindAllAsync(string sortProperty, SortOrder sortOrder, string searchText = "");
        Task<AsnDetail> FindAsyncById(int id);
        Task<IEnumerable<AsnDetail>> FindAsyncByAsnCode(string asnCode);
        void Create(AsnDetail asnDetail);
        void Update(AsnDetail asnDetail);
        void Delete(AsnDetail asnDetails);
        void Delete(List<AsnDetail> asnDetails);
    }
}
