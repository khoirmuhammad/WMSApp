﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WMSApplication.Models
{
    public class Unit
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public IList<Product> Products { get; set; }
    }
}
