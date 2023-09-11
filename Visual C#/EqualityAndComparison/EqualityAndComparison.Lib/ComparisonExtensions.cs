namespace EqualityAndComparison.Lib
{
  /// <summary>
  ///   Common Equality and Comparison operations.
  /// </summary>
  public static class ComparisonExtensions
  {
    public static bool AreEqualIgnoreCase(this string left, string right)
    {
      //This covers two cases: 1. Both are the same reference and 2. Both are null
      if (ReferenceEquals(left, right)) return true;

      if ((left == null) ^ (right == null)) return false;

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

      if ((primary == null) ^ (secondary == null)) return false;

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

    //Subjective comparison of Lists where elements do not have to be distinct.
    //Instead if one list is a subset of the other, then they can loosely be considered equal
    public static bool IsFuzzySubset<T>(
      this IReadOnlyList<T> left,
      IReadOnlyList<T> right,
      IEqualityComparer<T> comparer = null)
      where T : IEquatable<T>
    {
      if (left == null && right == null) return true;

      if ((left == null) ^ (right == null)) return false;

      var doubleCheck = false;
      IReadOnlyList<T> smaller;
      IReadOnlyList<T> larger;

      //Figuring out which list is larger, where equals is arbitrary
      if (left.Count <= right.Count)
      {
        smaller = left;
        larger = right;

        //If they are equal, unfortunately a double check has to be performed.
        doubleCheck = left.Count == right.Count;
      }
      else
      {
        smaller = right;
        larger = left;
      }

      var smallerVersusLarger = smaller.IsSubsetOf(larger);

      //If every element in larger is found in smaller, they these are loosely equal
      //if being distinct does not matter.
      //Smaller must be a subset of larger for this to be true.
      if (!doubleCheck) return smallerVersusLarger;

      //If the counts are equal, then both lists have to be compared to one another unfortunately
      var largerVersusSmaller = larger.IsSubsetOf(smaller);

      return smallerVersusLarger && largerVersusSmaller;
    }

    public static bool IsSubsetOf<T>(
      this IReadOnlyList<T> smaller,
      IReadOnlyList<T> larger,
      IEqualityComparer<T> comparer = null)
      where T : IEquatable<T>
    {
      //Compare larger to smaller by looping the larger list
      foreach (var item in larger)
      {
        //Search smaller list for a shorter trip
        if (smaller.Contains(item, comparer)) continue;

        //If an element from larger is not found in smaller, then these connot be considered equal
        return false;
      }

      return true;
    }
  }
}
