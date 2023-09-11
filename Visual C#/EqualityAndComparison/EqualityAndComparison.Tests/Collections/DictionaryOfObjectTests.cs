using EqualityAndComparison.Lib.Entities;
using NUnit.Framework;

namespace EqualityAndComparison.Tests.Collections
{
  /// <summary>
  /// The goal of these tests is to demonstrate that without implementing the proper interfaces,
  /// the Dictionary{TKey, TValue} class will not be able to perform comparison and equality
  /// operations. All tests in this Fixture will fail (while passing the test) to demonstrate that point.
  /// </summary>
  [TestFixture]
  public class DictionaryOfObjectTests
  {
    private Dictionary<DumbEntity, DumbEntity> _dict;

    [SetUp]
    public void Setup()
    {
      _dict = DummyData.GetThreeFlatEntities().ToDictionaryOfDumbEntity();
    }

    /// <summary> The Contains method will not find your object unless it knows how. </summary>
    [Test]
    public void DumbEntity_WhenIEqualityComparerIsNotImplemented_ThenContainsDoesNotFindEqualObject()
    {
      var key = DummyData.GetFlatEntity().ToDumbEntity();

      var actual = _dict.ContainsKey(key);

      Assert.IsFalse(actual);
    }

    /// <summary> The Remove method does not know what to remove unless it is told how. </summary>
    [Test]
    public void DumbEntity_WhenIEqualityComparerIsNotImplemented_ThenRemoveCannotRemoveEqualObject()
    {
      var expected = DummyData.GetTwoFlatEntities().ToDictionaryOfDumbEntity();

      var target = DummyData.GetFlatEntity().ToDumbEntity();

      var actual = _dict.Remove(target);

      Assert.IsFalse(actual);
      Assert.IsFalse(expected.SequenceEqual(_dict));
    }

    /// <summary>
    /// The IndexOf and LastIndexOf methods do not know how to equate objects to find the index unless
    /// they are told how.
    /// </summary>
    [Test]
    public void DumbEntity_WhenIEqualityComparerIsNotImplemented_ThenAddWillNotRaiseAnExceptionForDuplicateKeys()
    {
      var item = DummyData.GetFlatEntity().ToDumbEntity();

      _dict.Add(item, item);

      Assert.Pass("The fact that an exception is not raised means the the test passed to demonstrate failure.");
    }
  }
}
