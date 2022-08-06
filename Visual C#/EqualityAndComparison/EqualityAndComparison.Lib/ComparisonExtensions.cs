namespace EqualityAndComparison.Lib
{
    /// <summary>
    /// Common Equality and Comparison operations.
    /// </summary>
    public static class ComparisonExtensions
    {
        public static bool AreEqualIgnoreCase(this string left, string right)
        {
            if (left == null && right == null) return true;

            if (left == null ^ right == null) return false;

            return left.Equals(right, StringComparison.OrdinalIgnoreCase);
        }

        public static int GetListHashCode<T>(this IList<T> list, Func<T, T> transform = null)
            where T : IEquatable<T>
        {
            if (list == null) return -1;

            if (!list.Any()) return 0;

            //Using Sum will throw an OverflowException here
            var hc = list.Aggregate(0, (sum, x) => GetObjectHashCode(x, transform));

            return hc;
        }

        public static int GetObjectHashCode<T>(this T obj, Func<T, T> transform = null)
        {
            if (obj == null) return -1;

            transform ??= e => e;

            return transform(obj).GetHashCode();
        }

        //This comparison assumes no duplicates exist
        //Not a replacement for "System.Linq.Enumerable.SequenceEqual", but handles null base cases, SequenceEqual does not.
        public static bool AreDistinctListsEqual<T>(
          this IReadOnlyList<T> primary,
          IReadOnlyList<T> secondary,
          IEqualityComparer<T> comparer = null)
          where T : IEquatable<T>
        {
            if (primary == null && secondary == null) return true;

            if (primary == null ^ secondary == null) return false;

            if (primary.Count != secondary.Count) return false;

            return primary.SequenceEqual(secondary, comparer);

            //Was using this originally because I didn't know Enumerable.SequenceEqual existed
            //foreach (var item in primary)
            //{
            //    if (secondary.Contains(item, comparer)) continue;

            //    return false;
            //}

            //return true;
        }

        //TODO: Subjective usage of AreListsEqual() where elements do not have to be distinct
    }
}
