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
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException("Error");

            return input.First().ToString().ToUpper() + input.Substring(1);
        }

        public static string GenerateUniqueCode(string prefixCode, int lengthCode, int currSequence)
        {
            if (currSequence.Equals(0))
                throw new ArgumentException("Error");

            string uniqueCode = prefixCode;

            for (int i = 0; i < lengthCode - currSequence.ToString().Length; i++)
            {
                uniqueCode = uniqueCode + "0";
            }

            uniqueCode = uniqueCode + (currSequence + 1).ToString();

            return uniqueCode;
        }
    }
}
