using EqualityAndComparison.Lib.Entities;

namespace EqualityAndComparison.Lib
{
  public class FlatEntityComparer
    : IEqualityComparer<FlatEntity>
  {
    public bool Equals(FlatEntity left, FlatEntity right)
    {
      if (ReferenceEquals(left, right)) return true;

      if (left == null ^ right == null) return false;

      if (left.GetType() != right.GetType()) return false;

      return
        left.Age == right.Age &&
        left.Price == right.Price &&
        left.JobCode == right.JobCode &&
        left.SignificantDate.Equals(right.SignificantDate);
    }

    public int GetHashCode(FlatEntity obj) => HashCode.Combine(
      obj.Age,
      obj.Price,
      obj.JobCode,
      obj.SignificantDate);
  }
}
