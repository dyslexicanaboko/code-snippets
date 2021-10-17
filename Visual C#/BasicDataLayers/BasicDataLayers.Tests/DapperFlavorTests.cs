using BasicDataLayers.Lib.DapperFlavor;
using BasicDataLayers.Lib.Entities;
using NUnit.Framework;

namespace BasicDataLayers.Tests
{
    [TestFixture]
    public class DapperFlavorTests
        : BaseTest
    {
        [Test]
        public void InsertUpdateSelect()
        {
            var e = new RudimentaryEntity();
            var dapper = new DapperRepository();

            var pk = dapper.Insert(e);

            UpdateObject(e, pk);

            dapper.Update(e);

            var a = dapper.Select(e.PrimaryKey);

            AssertAreEqual(e, a);
        }
    }
}
