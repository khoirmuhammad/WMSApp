using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMSApplication.CustomModels;
using WMSApplication.Models;

namespace WMSApplication.Contracts
{
    public interface IAsnRepository
    {
        Task<IEnumerable<Asn>> FindAllAsync();
        Task<IEnumerable<Asn>> FindAllAsync(string sortProperty, SortOrder sortOrder, string searchText = "");
        Task<Asn> FindAsyncById(string code);
        void Create(Asn asn);
        void Update(Asn asn);
        void Delete(Asn asn);
    }
}
