using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WMSApplication.Models
{
    public class TransactionStatus
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Status { get; set; }
        public string TransactionType { get; set; }
        public string Description { get; set; }

    }
}
