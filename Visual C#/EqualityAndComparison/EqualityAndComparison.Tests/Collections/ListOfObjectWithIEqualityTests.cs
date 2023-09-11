using EqualityAndComparison.Lib.Entities;
using NUnit.Framework;

namespace EqualityAndComparison.Tests.Collections
{
  /// <summary>
  /// The goal of these tests is to demonstrate that the List{T} class depends on the object
  /// T to implement IEquatable{T} in order to perform search operations. The one exception
  /// is the Sort() method which does not require IEquatable{T} to be implemented, but instead
  /// requires Comparison{T} delegate, IComparable{T} or IComparer{T} to perform the sort.
  /// </summary>
  [TestFixture]
  public class ListOfObjectWithIEqualityTests
  {
    private List<FlatEntity> _lst;

    [SetUp]
    public void Setup()
    {
      _lst = DummyData.GetThreeFlatEntities();
    }

    /// <summary> The Contains method will not find your object unless it knows how. </summary>
    [Test]
    public void FlatEntity_WhenIEqualityIsImplemented_ThenContainsFindsEqualObject()
    {
      var find = DummyData.GetFlatEntity();

      //GetHashCode() is never called, it only uses IEquatable<T>.Equals(T)
      var actual = _lst.Contains(find);

      Assert.IsTrue(actual);
    }

    /// <summary> The Remove method does not know what to remove unless it is told how. </summary>
    [Test]
    public void FlatEntity_WhenIEqualityIsImplemented_ThenRemoveRemovesEqualObject()
    {
      var expected = DummyData.GetTwoFlatEntities();

      var find = DummyData.GetFlatEntity();

      var actual = _lst.Remove(find);

      Assert.IsTrue(actual);
      Assert.IsTrue(expected.SequenceEqual(_lst));
    }

    /// <summary> The IndexOf and LastIndexOf methods do not know how to equate objects to find the index unless they are told how. </summary>
    [Test]
    public void FlatEntity_WhenIEqualityIsImplemented_ThenIndexOfReturnsIndexOfEqualObject()
    {
      var find = DummyData.GetFlatEntity();

      var actual = _lst.IndexOf(find);

      Assert.AreEqual(0, actual);
    }

    /// <summary>
    /// The Sort method cannot sort anything unless it knows how. This method however cannot utilize IEquatable{T} because
    /// that is only for testing the equality of two objects. It requires the IComparable{T} interface to tell it how to
    /// sort items. Since there are multiple ways to sort items, the Sort method has multiple overloads to handle the different
    /// scenarios that one could encounter.
    /// </summary>
    [Test]
    public void FlatEntity_WhenIComparableIsImplemented_ThenObjectsWillBeSortedAccordingToTheClass()
    {
      var expected = DummyData.GetSortedFlatEntities();

      _lst.Sort();

      Assert.IsTrue(expected.SequenceEqual(_lst));
    }

    //Distinct() relies on the Equals method for its comparison
    [Test]
    public void FlatEntity_WhenIEqualityIsImplemented_ThenDistinctWillEliminateDuplicateObjects()
    {
      var expected = new List<FlatEntity> { DummyData.GetFlatEntity() };
      var lst = new List<FlatEntity>(5);
      
      for (var i = 0; i < 5; i++)
      {
        lst.Add(DummyData.GetFlatEntity());
      }
      
      var actual = lst.Distinct().ToList();
      
      Assert.AreEqual(1, actual.Count);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }

    //DistinctBy() relies on the Equals method for its comparison
    [Test]
    public void FlatEntity_WhenIEqualityIsImplemented_ThenDistinctByWillEliminateDuplicateObjects()
    {
      var expected = new List<FlatEntity> { DummyData.GetFlatEntity() };
      var lst = new List<FlatEntity>(5);

      for (var i = 0; i < 5; i++)
      {
        lst.Add(DummyData.GetFlatEntity());
      }

      var actual = lst.DistinctBy(x => x.JobCode).ToList();

      Assert.AreEqual(1, actual.Count);
      Assert.IsTrue(expected.SequenceEqual(actual));
    }
  }
}
