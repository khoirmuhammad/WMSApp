using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WMSApplication.Models
{
    public class Location : Timestamp
    {
        public string Code { get; set; }
        public int Floor { get; set; }
        public string RackAisle { get; set; }
        public int Level { get; set; }
        public int Pos { get; set; }
        public string Type { get; set; }
        public int MaximumPallet { get; set; }

    }
}
