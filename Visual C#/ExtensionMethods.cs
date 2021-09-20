using System;
using System.Collections.Generic;
using System.Data;

//I can't remember where I used some of this, but I like to keep uses of generics
namespace ExtensionMethods
{
    public static class ReaderMethods
    {
        public static IEnumerable<T> Select<T>(this IDataReader reader,
            Func<IDataReader, T> projection)
        {
            while (reader.Read()) yield return projection(reader);
        }
    }

    public static class StringMethods
    {
		//This was used to perform a contains like tSQL's "LIKE" clause - can't remember what I used it for anymore.
        public static bool ContainsIgnoreCase(this string source, string search)
        {
            if (string.IsNullOrEmpty(search) || string.IsNullOrEmpty(source))
                return false;

            return source.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }
}