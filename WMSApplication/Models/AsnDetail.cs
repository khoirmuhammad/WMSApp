using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WMSApplication.Models
{
    public class AsnDetail : Timestamp
    {
        public int Id { get; set; }
        public int LineNumber { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public int OriginalQty { get; set; }
        public string OriginalUnit { get; set; }
        public DateTime ProductionDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string LineStatus { get; set; }
        public string ReceivingArea { get; set; }
        public string Remark { get; set; }

        [Display(Name = "ASN Code")]
        public string AsnCode { get; set; }
        public Asn Asn { get; set; }
    }
}
