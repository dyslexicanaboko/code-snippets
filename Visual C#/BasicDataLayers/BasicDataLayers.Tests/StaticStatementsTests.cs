using BasicDataLayers.Lib.Entities;
using BasicDataLayers.Lib.StaticStatements;
using NUnit.Framework;

namespace BasicDataLayers.Tests
{
    [TestFixture]
    public class StaticStatementsTests
        : BaseTest
    {
        [Test]
        public void InsertUpdateSelect()
        {
            var e = new RudimentaryEntity();
            var repo = new RudimentaryRepository();

            var pk = repo.Insert(e);

            UpdateObject(e, pk);

            repo.Update(e);

            var a = repo.Select(e.PrimaryKey);

            AssertAreEqual(e, a);
        }
    }
}
