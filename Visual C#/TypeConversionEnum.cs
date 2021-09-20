using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

//I don't have fun remembering how to do any of this, so I would like to not have to.
//I have code somewhere that converts the flags of an enum to a list of enum so you can iterate through the enum, I would like to add that here at one point
namespace EnumExtensionMethods
{
    public static class ConversionExtensions
    {
        public static T ConvertStringToEnum<T>(this string target)
        {
            return (T)Enum.Parse(typeof(T), target);
        }

        public static T ConvertInt32ToEnum<T>(this int number)
        {
            return (T)Enum.ToObject(typeof(T), number);
        }

        public static T ConvertInt16ToEnum<T>(this short number)
        {
            return (T)Enum.ToObject(typeof(T), number);
        }

        public static string GetEnumValueAsString<T>(this T target) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("T must be an enumerated type");

            return Convert.ToInt32(target).ToString();
        }       
    }
}