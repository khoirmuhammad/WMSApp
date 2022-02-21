using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WMSApplication.Helpers
{
    public class StringHelper
    {
        public static string ToUpperFirstString(string input)
        {
            if (String.IsNullOrEmpty(input))
                throw new ArgumentException("ARGH!");

            return input.First().ToString().ToUpper() + input.Substring(1);
        }
    }
}
