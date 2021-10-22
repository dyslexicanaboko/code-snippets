using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

//I don't have fun remembering how to do any of this, so I would like to not have to.
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
		
		//Sometimes you just need to iterate through an enumeration, this will return the enumeration as an array of its values.
		//Values in this context refers to the individual enumeration members, not the potential integer.
		public static T[] GetEnumArray<T>() 
			where T : struct, IConvertible
		{
			if (!typeof(T).IsEnum)
				throw new ArgumentException("T must be an enumerated type");
				
			var t = typeof(T);
			
			var arr = (T[])Enum.GetValues(t);

			return arr;
		}

		//Converting an enumeration to a searchable dictionary where the key can be unchanged (natural), lower case or upper case as needed.
		//keyIsLowerCase = {null: Leave it alone (natrual), true: make key lower case, false: make key upper case}
		public static Dictionary<string, T> GetEnumDictionary<T>(bool? keyIsLowerCase = null)
			where T : struct, IConvertible
		{
			if (!typeof(T).IsEnum)
				throw new ArgumentException("T must be an enumerated type");

			var t = typeof(T);

			var names = Enum.GetNames(t);

			if (keyIsLowerCase.HasValue)
			{
				Func<string, string> f;

				if (keyIsLowerCase.Value)
                {
                    f = s => s.ToLower();
                }
                else
                {
                    f = s => s.ToUpper();
                }
				
				names = names.Select(x => f(x)).ToArray();
			}
			
			var values = (T[])Enum.GetValues(t);

			var dict = new Dictionary<string, T>(names.Length);

			for (var i = 0; i < names.Length; i++)
			{
				dict.Add(names[i], values[i]);
			}
			
			return dict;
		}
		
		//Intended for use with drop down lists, combo boxes or anything that has a Key/Value or Text/Value list paradigm.
		//This assumes that the enumeration can be converted to an integer which is sketchy.
		public Dictionary<string, int> GetEnumListItems<T>()
			where T : struct, IConvertible
		{
			if (!typeof(T).IsEnum)
				throw new ArgumentException("T must be an enumerated type");

			var t = typeof(T);

			var names = Enum.GetNames(t);

			var values = (T[])Enum.GetValues(t);

			var dict = new Dictionary<string, int>(names.Length);

			for (var i = 0; i < names.Length; i++)
			{
				dict.Add(names[i], Convert.ToInt32(values[i]));
			}

			return dict;
		}
    }
}