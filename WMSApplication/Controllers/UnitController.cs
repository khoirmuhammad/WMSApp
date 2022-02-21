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
        public async Task<IActionResult> Index(string sortExpression = "")
        {
            SortingModel sortingModel = new SortingModel();

            sortingModel.SetColumn(UnitModelConstant.Property.name);
            sortingModel.SetColumn(UnitModelConstant.Property.description);
            sortingModel.SortingParam(sortExpression);

            ViewData["SortingModel"] = sortingModel;

            string sortedProperty = StringHelper.ToUpperFirstString(sortingModel.SortedProperty);

            IEnumerable<Unit> units = await _repository.Unit.FindAllAsync(sortedProperty, sortingModel.SortedOrder);

            return View(units);
        }

    }
}
