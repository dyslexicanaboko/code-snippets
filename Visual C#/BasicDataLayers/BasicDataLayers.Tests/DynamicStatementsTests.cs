using BasicDataLayers.Lib.DynamicStatements;
using BasicDataLayers.Lib.Entities;
using BasicDataLayers.Lib.StaticStatements;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BasicDataLayers.Tests
{
    [TestFixture]
    public class DynamicStatementsTests
        : BaseTest
    {
        [Test]
        public void InsertUpdateSelect()
        {
            var e = new RudimentaryEntity();
            var lst = new List<RudimentaryEntity> { e };
            var bld = new AutoBuildDml();

            var insert = bld.GetInsertSql(lst, Schema, Table, nameof(RudimentaryEntity.PrimaryKey));

            insert.Sql += "; SELECT SCOPE_IDENTITY() AS PK;";

            int pk;

            using (var dr = ExecuteReaderText(insert.Sql, insert.Parameters))
            {
                pk = Convert.ToInt32(GetScalar(dr, "PK"));
            }

            UpdateObject(e, pk);

            var update = bld.GetUpdateSql(e, Schema, Table, nameof(RudimentaryEntity.PrimaryKey));

            bld.ExecuteNonQuery(update);

            var select = bld.GetSelectByPrimaryKeySql<RudimentaryEntity>(
                    Schema, 
                    Table, 
                    nameof(RudimentaryEntity.PrimaryKey),
                    pk);

            RudimentaryEntity a;

            using (var dr = ExecuteReaderText(select.Sql, select.Parameters))
            {
                dr.Read();

                a = ToEntity(dr);
            }

            AssertAreEqual(e, a);
        }

        //If there is too much crap in the table truncate it, it will take too long for this to execute
        //TRUNCATE TABLE dbo.RudimentaryEntity
        [Test]
        public void BulkInsertBulkUpdate()
        {
            var bld = new AutoBuildBulkCopy();
            var repo = new RudimentaryRepository();

            var c = new RudimentaryEntity();
            var lst = new List<RudimentaryEntity>(BulkOperationCap);

            for (var i = 0; i < lst.Capacity; i++)
            {
                //This is adding the same ref repeatedly, but the database doesn't know any different
                lst.Add(c);
            }

            bld.BulkInsert(lst, Schema, Table, nameof(RudimentaryEntity.PrimaryKey));

            //This is a crappy way to do this, but these are crappy tests so I don't care. Just making sure I select the same objects is all.
            var lstInserted = repo.SelectAll().TakeLast(lst.Capacity).ToList();

            foreach (var u in lstInserted)
            {
                UpdateObject(u);
            }
            
            bld.BulkUpdate(lstInserted, Schema, Table, nameof(RudimentaryEntity.PrimaryKey));

            var lstUpdated = repo.SelectAll().TakeLast(lst.Capacity).ToList();

            for (var i = 0; i < lstInserted.Count; i++)
            {
                var e = lstInserted[i];
                var a = lstUpdated[i];

                AssertAreEqual(e, a);
            }
        }
    }
}
