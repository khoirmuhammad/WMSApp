﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WMSApplication.CustomModels
{
    public class PagingModel
    {
        public PagingModel(int totalItems, int currentPage, int pageSize)
        {
            TotalItems = totalItems;
            CurrentPage = currentPage;
            PageSize = pageSize;

            int totalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)pageSize);
            TotalPages = totalPages;

            int startPage = currentPage - 5;
            int endPage = currentPage + 4;

            if (startPage <= 0)
            {
                endPage = endPage - (startPage - 1);
                startPage = 1;
            }

            if (endPage > totalPages)
            {
                endPage = totalPages;
                if (endPage > 10)
                {
                    startPage = endPage - 9;
                }
            }

            StartRecord = (CurrentPage - 1) * PageSize + 1;
            EndRecord = StartRecord - 1 + pageSize;

            if (EndRecord > TotalItems)
            {
                EndRecord = TotalItems;
            }

            if (TotalItems == 0)
            {
                StartPage = 0;
                StartRecord = 0;
                CurrentPage = 0;
                EndRecord = 0;
            }
            else
            {
                StartPage = startPage;
                EndPage = endPage;
            }
        }

        public List<SelectListItem> GetPageSize()
        {
            var pageSizes = new List<SelectListItem>();

            for(int lp = 5; lp <= 50; lp += 5)
            {
                if (lp == this.PageSize)
                {
                    pageSizes.Add(new SelectListItem( lp.ToString(), lp.ToString(), true ));
                }
                else
                {
                    pageSizes.Add(new SelectListItem(lp.ToString(), lp.ToString()));
                }
            }

            return pageSizes;
        }

        // Readonly Properties by private on setter.
        public int TotalItems { get; private set; } // All records / data had by datasource
        public int CurrentPage { get; private set; } // page being shown on each pagination
        public int PageSize { get; private set; } // page size can be 5, 10, 15 and so on
        public int TotalPages { get; private set; } // all of pages had by pagination
        public int StartPage { get; private set; }
        public int EndPage { get; private set; }
        public int StartRecord { get; private set; } // start record on each pagination
        public int EndRecord { get; private set; } // end record on each pagination

        // Public Properties
        public string Action { get; set; } = "Index";
        public string SearchText { get; set; }
        public string SortExpression { get; set; }
    }
}
