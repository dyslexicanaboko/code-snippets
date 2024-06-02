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
    public void FlatEntity_WhenIEqualityComparerProvided_ThenContainsKeyWillFindEqualKey()
    {
      var find = DummyData.GetFlatEntity();

      //The Dictionary will use the GetHashCode() and Equals() methods from the Comparer to perform the search
      //It will not use the methods from the object itself.
      var actual = _dict.ContainsKey(find);

      Assert.IsTrue(actual);
    }

    /// <summary> The ContainsValue method will not find your object unless it knows how. </summary>
    [Test]
    public void FlatEntity_WhenIEqualityComparerProvided_ThenContainsValueWillFindEqualValue()
    {
      var value = DummyData.GetFlatEntity();

      //Almost like ContainsKey, the Dictionary will use overridden Equals() method of TValue only to perform the search
      //The IEqualityComparer{TKey} is only used for TKey.
      var actual = _dict.ContainsValue(value);

      Assert.IsTrue(actual);
    }

    /// <summary> The Remove method does not know what to remove unless it is told how. </summary>
    [Test]
    public void FlatEntity_WhenIEqualityComparerProvided_ThenRemoveCannotRemoveEqualKey()
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
    public void FlatEntity_WhenIEqualityComparerProvided_ThenAddWillRaiseArgumentExceptionForDuplicateKeys()
    {
      var find = DummyData.GetFlatEntity();

      Assert.Throws<ArgumentException>(() => { _dict.Add(find, find); });
    }
  }
}
