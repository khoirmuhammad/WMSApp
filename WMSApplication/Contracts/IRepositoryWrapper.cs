﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WMSApplication.Contracts
{
    public interface IRepositoryWrapper
    {
        IUnitRepository Unit { get; }
        IProductCategoryRepository ProductCategory { get; }
        IProductRepository Product { get; }
        ILocationRepository Location { get; }
        IAsnRepository Asn { get; }
        IAsnDetailRepository AsnDetail { get; }

        Task SaveAsync();
    }
}
