using EqualityAndComparison.Lib;
using NUnit.Framework;

namespace EqualityAndComparison.Tests
{
    [TestFixture]
    public class GenericTests
        : TestBase
    {
        [Test]
        public void Non_distinct_lists_must_be_subsets_of_each_other()
        {
            var left = new List<int>() { 1, 2, 3 };
            var right = new List<int>() { 1, 2, 3, 3 };

            Assert.IsTrue(left.IsFuzzySubset(right));
        }

        [Test]
        public void Non_distinct_lists_are_not_subsets_if_all_elements_are_not_common()
        {
            var left = new List<int>() { 1, 2, 3, 4, 5 };
            var right = new List<int>() { 1, 2, 3, 3 };

            Assert.IsFalse(left.IsFuzzySubset(right));
        }

        [Test]
        public void Non_distinct_lists_of_equal_size_are_not_subsets_if_all_elements_are_not_common()
        {
            var left = new List<int>() { 1, 2, 3, 4 };
            var right = new List<int>() { 1, 2, 3, 3 };

            Assert.IsFalse(left.IsFuzzySubset(right));
        }
    }
}
