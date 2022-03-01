using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMSApplication.Constants;
using WMSApplication.Contracts;
using WMSApplication.CustomModels;
using WMSApplication.Helpers;
using WMSApplication.Models;

namespace WMSApplication.Controllers
{
    public class LocationController : Controller
    {
        private readonly IRepositoryWrapper _repository;
        public LocationController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        private int _pageSize = 5;
        public async Task<IActionResult> Index(string sortExpression = "", string searchText = "", int pageIndex = 1, int pageSize = 5)
        {
            SortingModel sorting = SortingSetup(sortExpression);

            IEnumerable<Location> locations = await _repository.Location.FindAllAsync(StringHelper.ToUpperFirstString(sorting.SortedProperty), sorting.SortedOrder, searchText);

            PagingModel paging = PagingSetup(sortExpression, pageIndex, pageSize, locations);

            locations = locations.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            ViewData["Sorting"] = sorting;
            ViewData["Paging"] = paging;
            ViewBag.SearchText = searchText;

            TempData["currentPage"] = pageIndex;

            return View(locations);
        }

        public async Task<IActionResult> Details(string code)
        {
            Location location = await _repository.Location.FindAsyncById(code);
            return View(location);
        }

        public IActionResult Create()
        {
            Location location = new Location();
            ViewBag.Type = Types();

            return View(location);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Location location)
        {
            try
            {
                location.Code = GenerateCode(location);
                location.CreatedBy = "admin";
                location.CreatedDate = DateTime.Now;
                location.ModifiedBy = "admin";
                location.ModifiedDate = DateTime.Now;

                _repository.Location.Create(location);
                await _repository.SaveAsync();
                TempData["success"] = $"{location.Code} Saved Successfully";

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["failed"] = "Saving Data Failed";

                return View();
            }
        }

        

        public async Task<IActionResult> Edit(string code)
        {
            Location location = await _repository.Location.FindAsyncById(code);
            ViewBag.Type = Types();
            TempData.Keep();
            return View(location);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Location location)
        {
            try
            {
                location.ModifiedBy = "admin";
                location.ModifiedDate = DateTime.Now;

                _repository.Location.Update(location);
                await _repository.SaveAsync();
                TempData["success"] = $"{location.Code} Modified Successfully";

            }
            catch (Exception ex)
            {
                TempData["failed"] = "Modifying Data Failed";

                return RedirectToAction(nameof(Edit), new { code = location.Code });
            }

            int currPage = 1;
            if (TempData["currentPage"] != null)
            {
                currPage = (int)TempData["currentPage"];
            }

            return RedirectToAction(nameof(Index), new { pageIndex = currPage, pageSize = _pageSize });
        }

        public async Task<IActionResult> Delete(string code)
        {
            Location location = await _repository.Location.FindAsyncById(code);
            TempData.Keep();
            return View(location);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Location location)
        {
            try
            {
                _repository.Location.Delete(location);
                await _repository.SaveAsync();
            }
            catch (Exception ex)
            {
                TempData["failed"] = "Deleting Data Failed";

                return View();
            }

            return RedirectToAction(nameof(Index));
        }

        #region Support Methods
        private static PagingModel PagingSetup(string sortExpression, int pageIndex, int pageSize, IEnumerable<Location> locations)
        {
            PagingModel paging = new PagingModel(locations.Count(), pageIndex, pageSize);
            paging.SortExpression = sortExpression;

            return paging;
        }

        private static SortingModel SortingSetup(string sortExpression)
        {
            SortingModel sortingModel = new SortingModel();

            sortingModel.SetColumn(LocationModelConstant.Property.code);
            sortingModel.SetColumn(LocationModelConstant.Property.floor);
            sortingModel.SetColumn(LocationModelConstant.Property.rackAisle);
            sortingModel.SetColumn(LocationModelConstant.Property.level);
            sortingModel.SetColumn(LocationModelConstant.Property.pos);
            sortingModel.SetColumn(LocationModelConstant.Property.type);
            sortingModel.SetColumn(LocationModelConstant.Property.maximumPallet);
            sortingModel.SortingParam(sortExpression);

            return sortingModel;
        }

        private string GenerateCode(Location location)
        {
            string code = string.Empty;

            string floor = location.Floor < 10 ? $"0{location.Floor.ToString()}" : location.Floor.ToString();
            string aisle = location.RackAisle;
            string level = location.Level < 10 ? $"0{location.Level.ToString()}" : location.Level.ToString();
            string pos = location.Pos < 10 ? $"0{location.Pos.ToString()}" : location.Pos.ToString();

            code = $"{floor}-{aisle}-{level}-{pos}";

            return code;
        }

        private List<SelectListItem> Types()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            items.Insert(0, new SelectListItem()
            {
                Value = string.Empty,
                Text = "--- Please Select ---"
            });

            items.Insert(1, new SelectListItem()
            {
                Value = "Receiving Area",
                Text = "Receiving Area"
            });

            items.Insert(2, new SelectListItem()
            {
                Value = "Storage Area",
                Text = "Storage Area"
            });

            items.Insert(3, new SelectListItem()
            {
                Value = "Shipping Area",
                Text = "Shipping Area"
            });

            items.Insert(4, new SelectListItem()
            {
                Value = "Return Area",
                Text = "Return Area"
            });

            items.Insert(5, new SelectListItem()
            {
                Value = "Damage Area",
                Text = "Damage Area"
            });

            return items;
        }
        #endregion
    }
}
