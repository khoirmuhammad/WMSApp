using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WMSApplication.Contracts;
using WMSApplication.CustomModels;
using WMSApplication.Data;
using WMSApplication.Helpers;
using WMSApplication.Models;

namespace WMSApplication.Controllers
{
    public class ReceivingController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly IRepositoryWrapper _repository;
        private readonly IWebHostEnvironment _webhost;
        public ReceivingController(IRepositoryWrapper repository, IWebHostEnvironment webhost, ApplicationContext context)
        {
            _repository = repository;
            _webhost = webhost;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var receiving = await _repository.Asn.FindAllAsync();
            return View(receiving);
        }

        public IActionResult Create()
        {
            Asn asn = new Asn();
            asn.AsnDetails.Add(new AsnDetail() { LineNumber = 1 });

            SetViewBagForm();

            return View(asn);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Asn asn)
        {
            try
            {
                string buttonAction = Convert.ToString(Request.Form["hdBbtnAction"]);

                List<AsnDetail> asnDetails = asn.AsnDetails.Take(asn.AsnDetails.Count - 1).ToList(); // leave last row

                asn.Code = GetReceivingCode();
                asn.DocName = asn.DocUploaded != null ? asn.Code + "_" + asn.DocUploaded.FileName : asn.DocName;
                asn.CreatedBy = "admin";
                asn.CreatedDate = DateTime.Now;
                asn.ModifiedBy = "admin";
                asn.ModifiedDate = DateTime.Now;

                if (asnDetails.Count() == 0)
                {
                    if (buttonAction.Equals("save"))
                    {
                        asn.Status = "1";
                        asn.AsnDetails = null; // Detail relationship should be null if won't to insert to database.
                    }
                    else if (buttonAction.Equals("submit"))
                    {
                        asn.Status = "2";

                        TempData["failed"] = "Saving Data Failed - Please Add ASN Details";

                        SetViewBagForm();

                        return View(asn);
                    }
                }
                else
                {
                    asn.Status = buttonAction.Equals("save") ? "1" : buttonAction.Equals("submit") ? "2" : string.Empty;
                    asn.AsnDetails = null; // Detail relationship should be null if won't to insert to database.
                }

                

                _repository.Asn.Create(asn);

                foreach (AsnDetail asnDetail in asnDetails)
                {
                    asnDetail.AsnCode = asn.Code;
                    asnDetail.CreatedBy = "admin";
                    asnDetail.CreatedDate = DateTime.Now;
                    asnDetail.ModifiedBy = "admin";
                    asnDetail.ModifiedDate = DateTime.Now;
                    asnDetail.LineStatus = asn.Status;

                    _repository.AsnDetail.Create(asnDetail);
                }

                await _repository.SaveAsync();
                UploadFile(asn);
                TempData["success"] = $"{asn.Code} Saved Successfully";
            }
            catch(Exception ex)
            {
                SetViewBagForm();

                TempData["failed"] = "Saving Data Failed";

                return View(asn);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string code)
        {
            Asn asn = await _repository.Asn.FindAsyncById(code);
            
            asn.AsnDetails.Add(new AsnDetail() { LineNumber = asn.AsnDetails.Count + 1 });

            asn.AsnDetails.OrderBy(x => x.LineNumber);

            SetViewBagForm();
            TempData.Keep();
            return View(asn);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Asn asn)
        {
            try
            {
                string buttonAction = Convert.ToString(Request.Form["hdBbtnAction"]);

                if (buttonAction.Equals("submit") && asn.AsnDetails.Count == 1)
                {
                    TempData["failed"] = "Saving Data Failed - Please Add ASN Details";
                    return RedirectToAction(nameof(Edit), new { code = asn.Code });
                }

                foreach(var item in asn.AsnDetails.Where(x => x.ProductCode == null).ToList())
                {
                    asn.AsnDetails.Remove(item);
                }

                // Set Value that not provided by screen
                asn.Status = buttonAction.Equals("Save As Draft") ? "1" : "2";
                asn.CreatedBy = "admin";
                asn.CreatedDate = DateTime.Now;
                asn.ModifiedBy = "admin";
                asn.ModifiedDate = DateTime.Now;

                // Set Value that not provided by screen
                asn.AsnDetails.ToList().ForEach(s => s.LineStatus = asn.Status);
                asn.AsnDetails.ToList().ForEach(s => s.AsnCode = asn.Code);
                asn.AsnDetails.ToList().ForEach(s => s.CreatedBy = "admin");
                asn.AsnDetails.ToList().ForEach(s => s.CreatedDate = DateTime.Now);
                asn.AsnDetails.ToList().ForEach(s => s.ModifiedBy = "admin");
                asn.AsnDetails.ToList().ForEach(s => s.ModifiedDate = DateTime.Now);

                List<AsnDetail> existAsnDetails = _repository.AsnDetail.FindAsyncByAsnCode(asn.Code).Result.ToList();
                //_context.AsnDetail.RemoveRange(existAsnDetails);
                //_context.SaveChanges();

                _repository.AsnDetail.Delete(existAsnDetails);
                await _repository.SaveAsync();

                //_context.Attach(asn);
                //_context.Entry(asn).State = EntityState.Modified;
                //_context.AsnDetail.AddRange(asn.AsnDetails);
                //_context.SaveChanges();

                _repository.Asn.Update(asn);
                foreach(var item in asn.AsnDetails)
                {
                    _repository.AsnDetail.Create(item);
                }
                await _repository.SaveAsync();


                UploadFile(asn);
                TempData["success"] = $"{asn.Code} Updated Successfully";
            }
            catch(Exception ex)
            {
                TempData["failed"] = "Updating Data Failed";
                return RedirectToAction(nameof(Edit), new { code = asn.Code });
            }

            return RedirectToAction(nameof(Index), new { code = asn.Code});
        }

        #region Support Methods



        [HttpGet]
        public async Task<string> GetProductName(string code)
        {
            var product = await _repository.Product.FindAsyncById(code);

            return product != null ? product.Name : string.Empty;
        }

        

        [HttpGet]
        public async Task<JsonResult> GetUnitOfProduct(string code)
        {
            List<UnitModel> uom = new List<UnitModel>();

            var product = await _repository.Product.FindAsyncById(code);

            if (product != null)
            {
                uom.Add(new UnitModel() { code = product.UnitCode });
                uom.Add(new UnitModel() { code = product.WholeUnit });
            }
            else
            {
                uom.Add(new UnitModel() { code = string.Empty }) ;
            }
            

            return Json(new SelectList(uom, "code", "code"));
        }

        private List<SelectListItem> Products()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            var products = _repository.Product.FindAllAsync().Result;

            items = products.OrderBy(x => x.Code).Select(pc => new SelectListItem()
            {
                Value = pc.Code,
                Text = pc.Code
            }).ToList();

            items.Insert(0, new SelectListItem()
            {
                Value = string.Empty,
                Text = "--- Please Select ---"
            });

            return items;
        }

        private List<SelectListItem> ReceivingAreas()
        {
            List<SelectListItem> items = new List<SelectListItem>();

            List<Location> receivingAreas = _repository.Location.FindAllAsync().Result.Where(x => x.Type.Equals("Receiving Area")).ToList();

            foreach (Location loc in receivingAreas)
            {
                items.Insert(0, new SelectListItem()
                {
                    Value = loc.Code,
                    Text = loc.Code
                });
            }

            items.Insert(0, new SelectListItem()
            {
                Value = string.Empty,
                Text = "--- Please Select ---"
            });

            return items;
        }

        private string GetReceivingCode()
        {
            //RCV070320221001
            string code = "RCV";
            string day = Convert.ToString(DateTime.Now.Day);
            string month = Convert.ToString(DateTime.Now.Month);
            string year = Convert.ToString(DateTime.Now.Year);

            code = $"{code}{day}{month}{year}";

            var data = _repository.Asn.FindAllAsync().Result.OrderByDescending(x => x.Code).Take(1);

            if (data.Count() < 1)
            {
                code = StringHelper.GenerateUniqueCode(code, 3, 0);
            }
            else
            {
                string lastCode = data.FirstOrDefault().Code;

                string lastSequence = lastCode[^3..]; // get 3 last characters

                code = StringHelper.GenerateUniqueCode(code, 3, Convert.ToInt32(lastSequence));
            }

            return code;
        }

        private void SetViewBagForm()
        {
            ViewBag.Products = Products();
            ViewBag.ReceivingAreas = ReceivingAreas();
        }

        private void UploadFile(Asn asn)
        {
            string uniqueFilename = string.Empty;
            string uploadFolder = string.Empty;
            string filePath = string.Empty;

            Asn existAsn = _repository.Asn.FindAsyncById(asn.Code).Result;

            if (asn.DocUploaded != null)
            {
                string existFile = existAsn != null ? existAsn.DocName : string.Empty;

                uploadFolder = Path.Combine(_webhost.WebRootPath, "receiving");
                uniqueFilename = asn.Code + "_" + asn.DocUploaded.FileName;
                filePath = Path.Combine(uploadFolder, uniqueFilename);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    asn.DocUploaded.CopyTo(fileStream);
                }

                // For Update Data
                if (!string.IsNullOrEmpty(existFile))
                {
                    DeleteFileInDirectory(existFile);
                }
            }
            
        }

        private void DeleteFileInDirectory(string existFile)
        {
            string uploadFolder = Path.Combine(_webhost.WebRootPath, "receiving");

            FileInfo file = new FileInfo(Path.Combine(uploadFolder, existFile));
            if (file != null)
            {
                file.Delete();
            }
        }

        #endregion
    }
}
