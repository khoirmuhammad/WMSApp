using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WMSApplication.Constants
{
    public class ProductModelConstant
    {
        /*
         * 1. Model has only single word such as (Code, Name, Description), then it should be lowercase (code, name, description)
         * 
         * 2. Model has more than 1 word such as (WholeUnit, WholeQty), then it should be lowercase for first character, and upper case for first character in subsequent word (wholeUnit, wholeQty)
         */
        public static class Property
        {
            public static string code = "code";
            public static string name = "name";
            public static string wholeUnit = "wholeUnit";
            public static string weight = "weight";
            public static string looseQty = "looseQty";
            public static string wholeQty = "wholeQty";
            public static string allocationType = "allocationType";
            public static string remark = "remark";
            public static string loosePrice = "LoosePrice";
            public static string wholePrice = "wholePrice";
            public static string categoryCode = "categoryCode";
            public static string unitCode = "unitCode";
        }

        public static class ViewData
        {
            public static string sortParamCode = "sortParamCode";
            public static string sortParamName = "sortParamName";
            public static string sortParamWholeUnit = "sortParamWholeUnit";
            public static string sortParamWeight = "sortParamWeight";
            public static string sortParamLooseQty = "sortParamLooseQty";
            public static string sortParamWholeQty = "sortParamWholeQty";
            public static string sortParamAllocationType = "sortParamAllocationType";
            public static string sortParamRemark = "sortParamRemark";
            public static string sortParamLoosePrice = "sortParamLoosePrice";
            public static string sortParamWholePrice = "sortParamWholePrice";
            public static string sortParamWholeCategoryCode = "sortParamWholeCategoryCode";
            public static string sortParamWholeUnitCode = "sortParamWholeUnitCode";

            public static string sortIconCode = "sortIconCode";
            public static string sortIconName = "sortIconName";
            public static string sortIconWholeUnit = "sortIconWholeUnit";
            public static string sortIconWeight = "sortIconWeight";
            public static string sortIconLooseQty = "sortIconLooseQty";
            public static string sortIconWholeQty = "sortIconWholeQty";
            public static string sortIconAllocationType = "sortIconAllocationType";
            public static string sortIconRemark = "sortIconRemark";
            public static string sortIconLoosePrice = "sortIconLoosePrice";
            public static string sortIconWholePrice = "sortIconWholePrice";
            public static string sortIconWholeCategoryCode = "sortIconWholeCategoryCode";
            public static string sortIconWholeUnitCode = "sortIconWholeUnitCode";
        }

        public static class Sort
        {
            public static string codeAscending = "code";
            public static string nameAscending = "name";
            public static string wholeUnitAscending = "wholeUnit";
            public static string weightAscending = "weight";
            public static string looseQtyAscending = "looseQty";
            public static string wholeQtyAscending = "wholeQty";
            public static string allocationTypeAscending = "allocationType";
            public static string remarkAscending = "remark";
            public static string loosePriceAscending = "LoosePrice";
            public static string wholePriceAscending = "wholePrice";
            public static string categoryCodeAscending = "categoryCode";
            public static string unitCodeAscending = "unitCode";

            public static string codeDescending = "code_desc";
            public static string nameDescending = "name_desc";
            public static string wholeUnitDescending = "wholeUnit_desc";
            public static string weightDescending = "weight_desc";
            public static string looseQtyDescending = "looseQty_desc";
            public static string wholeQtyDescending = "wholeQty_desc";
            public static string allocationTypeDescending = "allocationType_desc";
            public static string remarkDescending = "remark_desc";
            public static string loosePriceDescending = "LoosePrice_desc";
            public static string wholePriceDescending = "wholePrice_desc";
            public static string categoryCodeDescending = "categoryCode_desc";
            public static string unitCodeDescending = "unitCode_desc";
        }
    }
}
