using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

//Converting DataTable or DataReader to objects
namespace ConversionExtensionMethods
{
    public static class ConversionExtensions
    {
		//This is a concept I came up with I used together with older ADO.NET Data Layers I created that manufactured objects
		//It's old and not functional because a void method would take a reference to the object being filled
        public static List<T> ToList<T>(this DataTable dt, Action<T, DataRow> method) where T : new()
        {
            var obj = default(T);

            var lst = dt == null ? new List<T>() : new List<T>(dt.Rows.Count);

            foreach (DataRow dr in dt.Rows)
            {
                obj = new T();

                method(obj, dr);

                lst.Add(obj);
            }

            return lst;
        }

		//The evolution of the above which is functional, but still using DataTables which are inferior to DataReaders
        public static List<T> ToList<T>(this DataTable dt, Func<DataRow, T> method) where T : new()
        {
            var lst = dt == null ? new List<T>() : new List<T>(dt.Rows.Count);

            foreach (DataRow dr in dt.Rows)
                lst.Add(method(dr));

            return lst;
        }

		//Final evolution, functional, using DataReader. Doesn't get any faster. The only loss is not being able to new up the list with an exact capacity
        public static List<T> ToList<T>(this IDataReader reader, Func<IDataReader, T> method) where T : new()
        {
            var lst = new List<T>();

            if (reader == null)
                return lst;

            while (reader.Read())
                lst.Add(method(reader));

            return lst;
        }

		//I am sure I had a reason for this at one point
        public static HashSet<T> ToHashSet<T>(this IDataReader reader, Func<IDataReader, T> method,
            IEqualityComparer<T> equalityComparer) where T : new()
        {
            var hs = new HashSet<T>(equalityComparer);

            if (reader == null)
                return hs;

            while (reader.Read())
                hs.Add(method(reader));

            return hs;
        }

		//This is flawed in that it reads in a while loop. Should definitely make a change to this in the future.
        public static object GetScalar(this IDataReader reader, string column)
        {
            object obj = null;

            if (reader == null)
                return null;

            while (reader.Read())
                obj = reader[column];

            return obj;
        }

		//At one point I was using XML to pass arguments to stored procedures before learning about UDT/TVP
        public static string ToXmlList<T>(this List<T> listOfScalarItems, char elementCharacter = 'i')
        {
            var sb = new StringBuilder();

            sb.Append("<list>");

            if (listOfScalarItems != null)
            {
                var str = elementCharacter.ToString();

                listOfScalarItems.ForEach(x =>
                    sb.Append("<").Append(str).Append(">").Append(x).Append("</").Append(str).Append(">"));
            }

            sb.Append("</list>");

            return sb.ToString();
        }
    }
}