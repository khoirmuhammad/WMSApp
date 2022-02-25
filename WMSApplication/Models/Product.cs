using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WMSApplication.Models
{
    public class Product : Timestamp
    {
        public string Code { get; set; }
        public string Name { get; set; }

        [Display(Name = "Whole Unit")]
        public string WholeUnit { get; set; }
        public decimal Weight { get; set; }

        [Display(Name = "Loose Qty")]
        public int LooseQty { get; set; }

        [Display(Name = "Whole Qty")]
        public int WholeQty { get; set; }

        [Display(Name = "Allocation Type")]
        public string AllocationType { get; set; }
        public string Remark { get; set; }

        [Display(Name = "Loose Price")]
        public decimal LoosePrice { get; set; }

        [Display(Name = "Whole Price")]
        public decimal WholePrice { get; set; }

        public string Picture { get; set; }
        public string PictureExtension { get; set; }
        public string PicturePath { get; set; }

        [NotMapped]
        public IFormFile UploadedPicture { get; set; }

        [Display(Name = "Product Category")]
        public string CategoryCode { get; set; }
        public ProductCategory ProductCategory { get; set; }

        [Display(Name = "Unit")]
        public string UnitCode { get; set; }
        public Unit Unit { get; set; }
    }
}
