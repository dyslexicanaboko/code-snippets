using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

//I was obsessed at one point with just getting a value that was not null. I wouldn't use this anymore for database purposes like I used to
//however it still serves a purpose for things that cannot be null. Took a long time to write all this crap too.
namespace ConversionExtensionMethods
{
    public static class ConversionExtensions
    {
        public static string ConvertToString(this DataRow item, string columnName)
        {
            return ConvertToString(item[columnName]);
        }

        public static string ConvertToString(this object item)
        {
            return item == null ? string.Empty : Convert.ToString(item);
        }

        public static string ConvertToStringAndTrim(this DataRow item, string columnName)
        {
            return ConvertToStringAndTrim(item[columnName]);
        }

        public static string ConvertToStringAndTrim(this object item)
        {
            return item.ConvertToString().Trim();
        }

        public static bool ConvertToBoolean(this DataRow item, string columnName)
        {
            return ConvertToBoolean(item[columnName]);
        }

        public static bool ConvertToBoolean(this object item)
        {
            return ConvertToBoolean(ConvertToString(item));
        }

        public static bool ConvertToBoolean(this string item)
        {
            var converted = false;


            if (!string.IsNullOrWhiteSpace(item))
            {
                item = item.ToLower().Trim();


                if (!bool.TryParse(item, out converted))
                    if (item == "1")
                        converted = true;
            }

            return converted;
        }

        public static int ConvertToInt32(this DataRow item, string columnName)
        {
            return ConvertToInt32(item[columnName]);
        }

        public static int ConvertToInt32(this object item)
        {
            return ConvertToInt32(ConvertToString(item));
        }

        public static int ConvertToInt32(this string item)
        {
            var converted = 0;


            int.TryParse(item, out converted);

            return converted;
        }

        public static decimal ConvertToDecimal(this DataRow item, string columnName)
        {
            return ConvertToDecimal(item[columnName]);
        }

        public static decimal ConvertToDecimal(this object item)
        {
            return ConvertToDecimal(ConvertToString(item));
        }

        public static decimal ConvertToDecimal(this string item)
        {
            decimal converted = 0;


            decimal.TryParse(item, out converted);

            return converted;
        }

        public static double ConvertToDouble(this DataRow item, string columnName)
        {
            return ConvertToDouble(item[columnName]);
        }

        public static double ConvertToDouble(this object item)
        {
            return ConvertToDouble(ConvertToString(item));
        }

        public static double ConvertToDouble(this string item)
        {
            double converted = 0;


            double.TryParse(item, out converted);

            return converted;
        }

        public static DateTime ConvertToDateTime(this DataRow item, string columnName)
        {
            return ConvertToDateTime(item[columnName]);
        }

        public static DateTime ConvertToDateTime(this object item)
        {
            return ConvertToDateTime(ConvertToString(item));
        }

        public static DateTime ConvertToDateTime(this string item)
        {
            var converted = DateTime.MinValue;


            DateTime.TryParse(item, out converted);

            return converted;
        }        
    }
}