using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WMSApplication.Models
{
    public class Asn : Timestamp
    {
        //Advanced Shipping Notice
        [Display(Name = "ASN No")]
        public string Code { get; set; }

        [Display(Name = "PO No")]
        public string PONumber { get; set; }

        [Display(Name = "Supplier Name")]
        public string SupplierName { get; set; }

        [Display(Name = "Supplier Address")]
        public string SupplierAddress { get; set; }

        [Display(Name = "Supplier Email")]
        public string SupplierMail { get; set; }

        [Display(Name = "Supplier Phone")]
        public string SupplierPhone { get; set; }

        [Display(Name = "Receiving Status")]
        public string Status { get; set; }

        [NotMapped]
        public IFormFile DocUploaded { get; set; }

        [Display(Name = "Document Upload")]
        public string DocName { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public DateTime? ReceivingDate { get; set; }
        public string Remark { get; set; }

        public virtual IList<AsnDetail> AsnDetails { get; set; } = new List<AsnDetail>(); // in order to store detail asn lines
    }
}
