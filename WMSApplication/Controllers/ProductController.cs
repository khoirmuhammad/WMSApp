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
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace WMSApplication.Controllers
{
    public class ProductController : Controller
    {
        private readonly IRepositoryWrapper _repository;
        private readonly IWebHostEnvironment _webhost;

        public ProductController(IRepositoryWrapper repository, IWebHostEnvironment webhost)
        {
            _repository = repository;
            _webhost = webhost;
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
                product.Code = StringHelper.GenerateUniqueCode("P-", 5, 0); //"P-00001";
            }
            else
            {
                string lastCode = products.OrderByDescending(x => x.Code).Take(1).FirstOrDefault().Code;
                int currSequence = Convert.ToInt32(lastCode.Split("-")[1]);

                product.Code = StringHelper.GenerateUniqueCode("P-", 5, currSequence);
            }

            product.WholeUnit = "Box";

            SetViewBagOnCreate();

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
                Dictionary<string,string> imgProcessing = UploadPicture(product);

                if (imgProcessing.Count > 0)
                {
                    product.Picture = imgProcessing["picture"].ToString();
                    product.PictureExtension = imgProcessing["pictureExtension"].ToString();
                    product.PicturePath = imgProcessing["picturePath"].ToString();
                }

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
                SetViewBagOnCreate();
                return View(product);
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
                Dictionary<string, string> imgProcessing = UploadPicture(product);

                if (imgProcessing.Count > 0)
                {
                    product.Picture = imgProcessing["picture"].ToString();
                    product.PictureExtension = imgProcessing["pictureExtension"].ToString();
                    product.PicturePath = imgProcessing["picturePath"].ToString();
                }
                

                product.ModifiedBy = "admin";
                product.ModifiedDate = DateTime.Now;

                _repository.Product.Update(product);
                await _repository.SaveAsync();
                TempData["success"] = $"{product.Name} Modified Successfully";

            }
            catch (Exception ex)
            {
                TempData["failed"] = "Modifying Data Failed";

                return RedirectToAction(nameof(Edit), new { code = product.Code });
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
                string existPicture = _repository.Product.FindAsyncById(product.Code).Result.Picture;

                _repository.Product.Delete(product);
                await _repository.SaveAsync();

                if (!string.IsNullOrEmpty(existPicture))
                {
                    DeletePictureInDirectory(existPicture);
                }
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

            items.Insert(2, new SelectListItem()
            {
                Value = "LIFO",
                Text = "LIFO - Last In First Out"
            });

            items.Insert(3, new SelectListItem()
            {
                Value = "FEFO",
                Text = "FEFO - First Expired First Out"
            });

            return items;
        }

        private Dictionary<string, string> UploadPicture(Product product)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            string uniqueFilename = string.Empty;
            string uploadFolder = string.Empty;
            string filePath = string.Empty;

            Product existProduct = _repository.Product.FindAsyncById(product.Code).Result;

            if (product.UploadedPicture != null)
            { 
                string existPicture = existProduct != null ? existProduct.Picture : string.Empty;

                uploadFolder = Path.Combine(_webhost.WebRootPath, "products");
                uniqueFilename = product.Code + "_" + product.UploadedPicture.FileName;
                filePath = Path.Combine(uploadFolder, uniqueFilename);

                using(var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    product.UploadedPicture.CopyTo(fileStream);
                }

                // For Update Data
                if (!string.IsNullOrEmpty(existPicture))
                {
                    DeletePictureInDirectory(existPicture);
                }

                result["picture"] = uniqueFilename;
                result["pictureExtension"] = Path.GetExtension(product.UploadedPicture.FileName);
                result["picturePath"] = filePath;
            }
            else
            {
                if (existProduct != null)
                {
                    result["picture"] = existProduct.Picture;
                    result["pictureExtension"] = existProduct.PictureExtension;
                    result["picturePath"] = existProduct.PicturePath;
                }
            }

            return result;
        }

        private void DeletePictureInDirectory(string existPicture)
        {
            string uploadFolder = Path.Combine(_webhost.WebRootPath, "products");

            FileInfo pict = new FileInfo(Path.Combine(uploadFolder, existPicture));
            if (pict != null)
            {
                pict.Delete();
            }
        }

        private void SetViewBagOnCreate()
        {
            ViewBag.ProductCategories = ProductCategories();
            ViewBag.Units = Units();
            ViewBag.AllocationType = AllocationTypes();
        }
        #endregion
    }
}
