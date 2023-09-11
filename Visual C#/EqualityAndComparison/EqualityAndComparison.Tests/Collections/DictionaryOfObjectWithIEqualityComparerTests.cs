using EqualityAndComparison.Lib;
using EqualityAndComparison.Lib.Entities;
using NUnit.Framework;

namespace EqualityAndComparison.Tests.Collections
{
  /// <summary>
  /// The goal of these tests is to demonstrate that the Dictionary{TKey, TValue} class can use an optional
  /// IEqualityComparer{TKey} constructor argument to perform search operations.
  /// </summary>
  [TestFixture]
  public class DictionaryOfObjectWithIEqualityComparerTests
  {
    private Dictionary<FlatEntity, FlatEntity> _dict;

    [SetUp]
    public void Setup()
    {
      //The Dictionary upon being initialized will use the GetHashCode() method of the objects provided to it
      _dict = DummyData.GetThreeFlatEntities().ToDictionary(new FlatEntityComparer());
    }

    /// <summary> The Contains method not find your object unless it knows how. </summary>
    [Test]
    public void FlatEntity_WhenIEqualityComparerImplemented_ThenContainsKeyWillFindEqualKey()
    {
      var find = DummyData.GetFlatEntity();

      //The Dictionary will use the GetHashCode() and Equals() methods from the Comparer to perform the search
      //It will not use the methods from the object itself.
      var actual = _dict.ContainsKey(find);

      Assert.IsTrue(actual);
    }

    /// <summary> The Remove method does not know what to remove unless it is told how. </summary>
    [Test]
    public void FlatEntity_WhenIEqualityComparerImplemented_ThenRemoveCannotRemoveEqualKey()
    {
      var expected = DummyData.GetTwoFlatEntities().ToDictionary();

      var find = DummyData.GetFlatEntity();

      var actual = _dict.Remove(find);

      Assert.IsTrue(actual);
      Assert.IsTrue(expected.SequenceEqual(_dict));
    }

    /// <summary>
    /// The IndexOf and LastIndexOf methods do not know how to equate objects to find the index unless
    /// they are told how.
    /// </summary>
    [Test]
    public void FlatEntity_WhenIEqualityComparerImplemented_ThenAddWillRaiseArgumentExceptionForDuplicateKeys()
    {
      var find = DummyData.GetFlatEntity();

      Assert.Throws<ArgumentException>(() => { _dict.Add(find, find); });
    }
  }
}
