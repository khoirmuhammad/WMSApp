using Microsoft.AspNetCore.Mvc;
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
    public class ProductCategoryController : Controller
    {
        private readonly IRepositoryWrapper _repository;

        public ProductCategoryController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }


        private int _pageSize = 5;
        public async Task<IActionResult> Index(string sortExpression = "", string searchText = "", int pageIndex = 1, int pageSize = 5)
        {
            SortingModel sorting = SortingSetup(sortExpression);

            IEnumerable<ProductCategory> productCategories = await _repository.ProductCategory.FindAllAsync(StringHelper.ToUpperFirstString(sorting.SortedProperty), sorting.SortedOrder, searchText);

            PagingModel paging = PagingSetup(sortExpression, pageIndex, pageSize, productCategories);

            productCategories = productCategories.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            ViewData["Sorting"] = sorting;
            ViewData["Paging"] = paging;
            ViewBag.SearchText = searchText;

            TempData["currentPage"] = pageIndex;

            return View(productCategories);
        }

        public async Task<IActionResult> Details(string code)
        {
            ProductCategory productCategory = await _repository.ProductCategory.FindAsyncById(code);
            return View(productCategory);
        }

        public async Task<IActionResult> Create()
        {
            ProductCategory productCategory = new ProductCategory();

            IEnumerable<ProductCategory> productCategories = await _repository.ProductCategory.FindAllAsync();

            if (productCategories.Count().Equals(0))
            {
                productCategory.Code = "C-001";
            }
            else
            {
                string lastCode = productCategories.OrderByDescending(x => x.Code).Take(1).FirstOrDefault().Code;
                int currSequence = Convert.ToInt32(lastCode.Split("-")[1]);

                productCategory.Code = StringHelper.GenerateUniqueCode("C-", 3, currSequence);
            }

            return View(productCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCategory productCategory)
        {
            try
            {
                _repository.ProductCategory.Create(productCategory);
                await _repository.SaveAsync();
                TempData["success"] = $"{productCategory.Name} Saved Successfully";
            }
            catch (Exception ex)
            {
                TempData["failed"] = "Saving Data Failed";

                return View();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string code)
        {
            ProductCategory productCategory = await _repository.ProductCategory.FindAsyncById(code);
            TempData.Keep();
            return View(productCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductCategory productCategory)
        {
            try
            {
                _repository.ProductCategory.Update(productCategory);
                await _repository.SaveAsync();
                TempData["success"] = $"{productCategory.Name} Modified Successfully";

            }
            catch (Exception ex)
            {
                TempData["failed"] = "Modifying Data Failed";

                return View();
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
            ProductCategory productCategory = await _repository.ProductCategory.FindAsyncById(code);
            TempData.Keep();
            return View(productCategory);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(ProductCategory productCategory)
        {
            try
            {
                _repository.ProductCategory.Delete(productCategory);
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
        private static PagingModel PagingSetup(string sortExpression, int pageIndex, int pageSize, IEnumerable<ProductCategory> productCategories)
        {
            PagingModel paging = new PagingModel(productCategories.Count(), pageIndex, pageSize);
            paging.SortExpression = sortExpression;

            return paging;
        }

        private static SortingModel SortingSetup(string sortExpression)
        {
            SortingModel sortingModel = new SortingModel();

            sortingModel.SetColumn(ProdcutCategoryModelConstant.Property.code);
            sortingModel.SetColumn(ProdcutCategoryModelConstant.Property.name);
            sortingModel.SetColumn(ProdcutCategoryModelConstant.Property.description);
            sortingModel.SortingParam(sortExpression);

            return sortingModel;
        }
        #endregion
    }
}
