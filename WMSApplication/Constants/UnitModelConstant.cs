using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WMSApplication.Constants
{
    public class UnitModelConstant
    {
        public static class Property
        {
            public static string name = "name";
            public static string description = "description";
        }

        public static class ViewData
        {
            public static string sortParamName = "sortParamName";
            public static string sortParamDescription = "sortParamDescription";

            public static string sortIconName = "sortIconName";
            public static string sortIconDescription = "sortIconDescription";
        }

        public static class Sort
        {
            public static string nameAscending = "name";
            public static string descriptionAscending = "desc";

            public static string nameDescending = "name_desc";
            public static string descriptionDescending = "desc_desc";
        }
    }
}
