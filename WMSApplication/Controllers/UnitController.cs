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

        private int _pageSize = 5;

        public async Task<IActionResult> Index(string sortExpression = "", string searchText = "", int pageIndex = 1, int pageSize = 5)
        {
            SortingModel sorting = SortingSetup(sortExpression);

            IEnumerable<Unit> units = await _repository.Unit.FindAllAsync(StringHelper.ToUpperFirstString(sorting.SortedProperty), sorting.SortedOrder, searchText);

            PagingModel paging = PagingSetup(sortExpression, pageIndex, pageSize, units);

            units = units.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            ViewData["Sorting"] = sorting;
            ViewData["Paging"] = paging;
            ViewBag.SearchText = searchText;

            TempData["currentPage"] = pageIndex;

            return View(units);
        }

        public async Task<IActionResult> Details(int id)
        {
            Unit unit = await _repository.Unit.FindAsyncById(id);
            return View(unit);
        }

        public IActionResult Create()
        {
            Unit unit = new Unit();

            return View(unit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Unit unit)
        {
            try
            {
                _repository.Unit.Create(unit);
                await _repository.SaveAsync();
                TempData["success"] = $"{unit.Name} Saved Successfully";

                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                TempData["failed"] = ex.Message;

                return View();
            }

            
        }

        public async Task<IActionResult> Edit(int id)
        {
            Unit unit = await _repository.Unit.FindAsyncById(id);
            TempData.Keep();
            return View(unit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Unit unit)
        {
            try
            {
                _repository.Unit.Update(unit);
                await _repository.SaveAsync();
                TempData["success"] = $"{unit.Name} Modified Successfully";
                
            }
            catch(Exception ex)
            {

            }

            int currPage = 1;
            if (TempData["currentPage"] != null)
            {
                currPage = (int)TempData["currentPage"];
            }

            return RedirectToAction(nameof(Index), new { pageIndex = currPage, pageSize = _pageSize });
        }

        public async Task<IActionResult> Delete(int id)
        {
            Unit unit = await _repository.Unit.FindAsyncById(id);
            TempData.Keep();
            return View(unit);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Unit unit)
        {
            try
            {
                _repository.Unit.Delete(unit);
                await _repository.SaveAsync();
            }
            catch(Exception ex)
            {

            }

            return RedirectToAction(nameof(Index));
        }

        #region Support Methods
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
        #endregion
    }
}
