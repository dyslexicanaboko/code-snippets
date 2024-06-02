using EqualityAndComparison.Lib.Entities;
using NUnit.Framework;

namespace EqualityAndComparison.Tests.Collections
{
  /// <summary>
  /// The goal of these tests is to demonstrate that without implementing the proper interfaces, the List{T}
  /// class will not be able to perform comparison and equality operations. All tests in this Fixture will
  /// fail (while passing the test) to demonstrate that point.
  /// </summary>
  [TestFixture]
  public class ListOfObjectTests
  {
    private List<DumbEntity> _lst;

    [SetUp]
    public void Setup()
    {
      _lst = DummyData.GetThreeFlatEntities().ToDumbEntities();
    }

    /// <summary> The Contains method will not find your object unless it knows how. </summary>
    [Test]
    public void DumbEntity_WhenIEqualityIsNotImplemented_ThenContainsDoesNotFindEqualObject()
    {
      var find = DummyData.GetFlatEntity().ToDumbEntity();

      var actual = _lst.Contains(find);

      Assert.IsFalse(actual);
    }

    /// <summary> The Remove method does not know what to remove unless it is told how. </summary>
    [Test]
    public void DumbEntity_WhenIEqualityIsNotImplemented_ThenRemoveCannotRemoveEqualObject()
    {
      var expected = DummyData.GetTwoFlatEntities().ToDumbEntities();

      var find = DummyData.GetFlatEntity().ToDumbEntity();

      var actual = _lst.Remove(find);

      Assert.IsFalse(actual);
      Assert.IsFalse(expected.SequenceEqual(_lst));
    }

    /// <summary>
    /// The IndexOf and LastIndexOf methods do not know how to equate objects to find the index unless
    /// they are told how.
    /// </summary>
    [Test]
    public void DumbEntity_WhenIEqualityIsNotImplemented_ThenIndexOfWillNotReturnIndexOfEqualObject()
    {
      var find = DummyData.GetFlatEntity().ToDumbEntity();

      var actual = _lst.IndexOf(find);

      Assert.AreNotEqual(0, actual);
    }

    /// <summary>
    /// The Sort method cannot sort anything unless it knows how. This method however cannot utilize IEquatable{T} because
    /// that is only for testing the equality of two objects. It requires the IComparable{T} interface to tell it how to
    /// sort items. When IComparable{T} is not implemented then Sort will attempt to use the default compare, but if
    /// that doesn't work, then an exception is thrown.
    /// </summary>
    [Test]
    public void DumbEntity_WhenIComparableIsNotImplemented_ThenObjectsCannotBeSortedAndAnExceptionIsThrownInstead()
    {
      var expected = DummyData.GetSortedFlatEntities().ToDumbEntities();

      Assert.Throws<InvalidOperationException>(() => { _lst.Sort(); });
      
      Assert.IsFalse(expected.SequenceEqual(_lst));
    }

    [Test]
    public void DumbEntity_WhenIEqualityIsNotImplemented_ThenDistinctWillNotEliminateDuplicateObjects()
    {
      var expected = new List<DumbEntity> { DummyData.GetFlatEntity().ToDumbEntity() };
      var lst = new List<DumbEntity>(5);

      for (var i = 0; i < 5; i++)
      {
        lst.Add(DummyData.GetFlatEntity().ToDumbEntity());
      }

      var actual = lst.Distinct().ToList();

      Assert.AreNotEqual(1, actual.Count);
      Assert.IsFalse(expected.SequenceEqual(actual));
    }
  }
}
