using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMSApplication.Contracts;
using WMSApplication.Models;
using WMSApplication.Constants;
using WMSApplication.Helpers;
using WMSApplication.CustomModels;

namespace WMSApplication.Controllers
{
    public class UnitController : Controller
    {
        private readonly IRepositoryWrapper _repository;
        public UnitController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }
        public async Task<IActionResult> Index(string sortExpression = "", string searchText = "", int pageIndex = 1, int pageSize = 5)
        {
            SortingModel sorting = SortingSetup(sortExpression);

            IEnumerable<Unit> units = await _repository.Unit.FindAllAsync(StringHelper.ToUpperFirstString(sorting.SortedProperty), sorting.SortedOrder, searchText);

            PagingModel paging = PagingSetup(sortExpression, pageIndex, pageSize, units);

            units = units.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            ViewData["Sorting"] = sorting;
            ViewData["Paging"] = paging;
            ViewBag.SearchText = searchText;

            return View(units);
        }

        private static PagingModel PagingSetup(string sortExpression, int pageIndex, int pageSize, IEnumerable<Unit> units)
        {
            PagingModel paging = new PagingModel(units.Count(), pageIndex, pageSize);
            paging.SortExpression = sortExpression;

            return paging;
        }

        private static SortingModel SortingSetup(string sortExpression)
        {
            SortingModel sortingModel = new SortingModel();

            sortingModel.SetColumn(UnitModelConstant.Property.name);
            sortingModel.SetColumn(UnitModelConstant.Property.description);
            sortingModel.SortingParam(sortExpression);

            return sortingModel;
        }
    }
}
