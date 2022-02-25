using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMSApplication.Contracts;
using WMSApplication.CustomModels;
using WMSApplication.Helpers;
using WMSApplication.Models;
using WMSApplication.Constants;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WMSApplication.Controllers
{
    public class ProductController : Controller
    {
        private readonly IRepositoryWrapper _repository;

        public ProductController(IRepositoryWrapper repository)
        {
            _repository = repository;
        }

        private int _pageSize = 5;
        public async Task<IActionResult> Index(string sortExpression = "", string searchText = "", int pageIndex = 1, int pageSize = 5)
        {
            SortingModel sorting = SortingSetup(sortExpression);

            IEnumerable<Product> products = await _repository.Product.FindAllAsync(StringHelper.ToUpperFirstString(sorting.SortedProperty), sorting.SortedOrder, searchText);

            PagingModel paging = PagingSetup(sortExpression, pageIndex, pageSize, products);

            products = products.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            ViewData["Sorting"] = sorting;
            ViewData["Paging"] = paging;
            ViewBag.SearchText = searchText;

            TempData["currentPage"] = pageIndex;

            return View(products);
        }

        public IActionResult Create()
        {
            Product product = new Product();

            IEnumerable<Product> products = _repository.Product.FindAllAsync().Result;

            if (products.Count().Equals(0))
            {
                product.Code = "P-00001";
            }
            else
            {
                string lastCode = products.OrderByDescending(x => x.Code).Take(1).FirstOrDefault().Code;
                int currSequence = Convert.ToInt32(lastCode.Split("-")[1]);

                product.Code = StringHelper.GenerateUniqueCode("P-", 5, currSequence);
            }

            product.WholeUnit = "Box";

            
            ViewBag.ProductCategories = ProductCategories();
            ViewBag.Units = Units();
            ViewBag.AllocationType = AllocationTypes();

            return View(product);
        }

        public async Task<IActionResult> Details(string code)
        {
            Product product = await _repository.Product.FindAsyncById(code);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            try
            {
                product.CreatedBy = "admin";
                product.ModifiedBy = "admin";
                product.CreatedDate = DateTime.Now;
                product.ModifiedDate = DateTime.Now;

                _repository.Product.Create(product);
                await _repository.SaveAsync();
                TempData["success"] = $"{product.Name} Saved Successfully";
            }
            catch(Exception ex)
            {
                TempData["failed"] = "Saving Data Failed";

                return View();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string code)
        {
            Product product = await _repository.Product.FindAsyncById(code);

            ViewBag.ProductCategories = ProductCategories();
            ViewBag.Units = Units();
            ViewBag.AllocationType = AllocationTypes();

            TempData.Keep();
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product product)
        {
            try
            {
                product.ModifiedBy = "admin";
                product.ModifiedDate = DateTime.Now;

                _repository.Product.Update(product);
                await _repository.SaveAsync();
                TempData["success"] = $"{product.Name} Modified Successfully";

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
            Product product = await _repository.Product.FindAsyncById(code);
            TempData.Keep();
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Product product)
        {
            try
            {
                _repository.Product.Delete(product);
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
        private static PagingModel PagingSetup(string sortExpression, int pageIndex, int pageSize, IEnumerable<Product> products)
        {
            PagingModel paging = new PagingModel(products.Count(), pageIndex, pageSize);
            paging.SortExpression = sortExpression;

            return paging;
        }

        private static SortingModel SortingSetup(string sortExpression)
        {
            SortingModel sortingModel = new SortingModel();

            sortingModel.SetColumn(ProductModelConstant.Property.code);
            sortingModel.SetColumn(ProductModelConstant.Property.name);
            sortingModel.SetColumn(ProductModelConstant.Property.wholeUnit);
            sortingModel.SetColumn(ProductModelConstant.Property.weight);
            sortingModel.SetColumn(ProductModelConstant.Property.looseQty);
            sortingModel.SetColumn(ProductModelConstant.Property.wholeQty);
            sortingModel.SetColumn(ProductModelConstant.Property.allocationType);
            sortingModel.SetColumn(ProductModelConstant.Property.remark);
            sortingModel.SetColumn(ProductModelConstant.Property.loosePrice);
            sortingModel.SetColumn(ProductModelConstant.Property.wholePrice);
            sortingModel.SetColumn(ProductModelConstant.Property.categoryCode);
            sortingModel.SetColumn(ProductModelConstant.Property.unitCode);
            sortingModel.SortingParam(sortExpression);

            return sortingModel;
        }

        private List<SelectListItem> ProductCategories()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            var productCategories = _repository.ProductCategory.FindAllAsync().Result;

            items = productCategories.OrderBy(x => x.Code).Select(pc => new SelectListItem()
            {
                Value = pc.Code,
                Text = $"{pc.Code} - {pc.Name}"
            }).ToList();

            items.Insert(0, new SelectListItem()
            {
                Value = string.Empty,
                Text = "--- Please Select ---"
            });

            return items;
        }

        private List<SelectListItem> Units()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            var units = _repository.Unit.FindAllAsync().Result;

            items = units.OrderBy(x => x.Code).Select(pc => new SelectListItem()
            {
                Value = pc.Code,
                Text = $"{pc.Code} - {pc.Name}"
            }).ToList();

            items.Insert(0, new SelectListItem()
            {
                Value = string.Empty,
                Text = "--- Please Select ---"
            });

            return items;
        }

        private List<SelectListItem> AllocationTypes()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            items.Insert(0, new SelectListItem()
            {
                Value = string.Empty,
                Text = "--- Please Select ---"
            });

            items.Insert(1, new SelectListItem()
            {
                Value = "FIFO",
                Text = "FIFO - First In First Out"
            });

            items.Insert(1, new SelectListItem()
            {
                Value = "LIFO",
                Text = "LIFO - Last In First Out"
            });

            items.Insert(1, new SelectListItem()
            {
                Value = "FEFO",
                Text = "FEFO - First Expired First Out"
            });

            return items;
        }
        #endregion
    }
}
