using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WMSApplication.Constants
{
    public class LocationModelConstant
    {
        public static class Property
        {
            public static string code = "code";
            public static string floor = "floor";
            public static string rackAisle = "rackAisle";
            public static string level = "level";
            public static string pos = "pos";
            public static string type = "type";
            public static string maximumPallet = "maximumPallet";
        }

        public static class ViewData
        {
            public static string sortParamCode = "sortParamCode";
            public static string sortParamFloor = "sortParamFloor";
            public static string sortParamRackAisle = "sortParamRackAisle";
            public static string sortParamLevel = "sortParamLevel";
            public static string sortParamLoosePos = "sortParamLoosePos";
            public static string sortParamType = "sortParamType";
            public static string sortParamMaximumPallet = "sortParamMaximumPallet";

            public static string sortIconCode = "sortIconCode";
            public static string sortIconFloor = "sortIconFloor";
            public static string sortIconRackAisle = "sortIconRackAisle";
            public static string sortIconLevel = "sortIconLevel";
            public static string sortIconLoosePos = "sortIconLoosePos";
            public static string sortIconType = "sortIconType";
            public static string sortIconMaximumPallet = "sortIconMaximumPallet";
        }

        public static class Sort
        {
            public static string codeAscending = "code";
            public static string floorAscending = "floor";
            public static string rackAisleAscending = "rackAisle";
            public static string levelAscending = "level";
            public static string posAscending = "pos";
            public static string typeAscending = "type";
            public static string maximumPalletAscending = "maximumPallet";

            public static string codeDescending = "code_desc";
            public static string floorDescending = "floo_desc";
            public static string rackAisleDescending = "rackAisle_desc";
            public static string levelDescending = "level_desc";
            public static string posDescending = "pos_desc";
            public static string typeDescending = "type_desc";
            public static string maximumPalletDescending = "maximumPallet_desc";
        }
    }
}
