namespace EqualityAndComparison.Lib
{
    /// <summary>
    /// Common Equality and Comparison operations.
    /// </summary>
    public static class ComparisonExtensions
    {
        public static int GetListHashCode<T>(this IList<T> list, Func<T, T> transform = null)
            where T : IEquatable<T>
        {
            if (list == null) return -1;

            if (!list.Any()) return 0;

            transform ??= e => e;

            //Using Sum will throw an OverflowException here
            var hc = list.Aggregate(0, (sum, x) => GetObjectHashCode(transform(x)));

            return hc;
        }

        public static int GetObjectHashCode(this object obj)
        {
            if (obj == null) return -1;

            return obj.GetHashCode();
        }

        //This comparison assumes no duplicates exist
        //Not a replacement for "System.Linq.Enumerable.SequenceEqual"
        public static bool AreDistinctListsEqual<T>(
          this IReadOnlyList<T> primary,
          IReadOnlyList<T> secondary,
          IEqualityComparer<T> comparer = null)
          where T : IEquatable<T>
        {
            if (primary == null && secondary == null) return true;

            if (primary == null ^ secondary == null) return false;

            if (primary.Count != secondary.Count) return false;

            foreach (var item in primary)
            {
                if (secondary.Contains(item, comparer)) continue;

                return false;
            }

            return true;
        }
    }
}
