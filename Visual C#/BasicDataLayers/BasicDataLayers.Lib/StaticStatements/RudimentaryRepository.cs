using BasicDataLayers.Lib.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BasicDataLayers.Lib.StaticStatements
{
	public class RudimentaryRepository
		: BaseDal
	{
		public RudimentaryEntity Select(int primaryKey)
		{
			var sql = @"
			SELECT
				 PrimaryKey
				,DollarAmount
				,ForeignKey
				,IsYes
				,Label
				,LuckyNumber
				,MathCalculation
				,ReferenceId
				,RightNow
			FROM dbo.RudimentaryEntity
			WHERE PrimaryKey = @PrimaryKey";

			var p = GetPrimaryKeyParameter(primaryKey);

			using (var dr = ExecuteReaderText(sql, p))
			{
				var lst = ToList(dr, ToEntity);

				if (!lst.Any()) return null;

				var entity = lst.Single();

				return entity;
			}
		}

		public IEnumerable<RudimentaryEntity> SelectAll()
		{
			var sql = @"
			SELECT
				 PrimaryKey
				,DollarAmount
				,ForeignKey
				,IsYes
				,Label
				,LuckyNumber
				,MathCalculation
				,ReferenceId
				,RightNow
			FROM dbo.RudimentaryEntity";

			using (var dr = ExecuteReaderText(sql))
			{
				var lst = ToList(dr, ToEntity);

				return lst;
			}
		}

		//Preference on whether or not insert method returns a value is up to the user and the object being inserted
		public int Insert(RudimentaryEntity entity)
		{
			var sql = @"INSERT INTO dbo.RudimentaryEntity (
				 DollarAmount
				,ForeignKey
				,IsYes
				,Label
				,LuckyNumber
				,MathCalculation
				,ReferenceId
				,RightNow) VALUES (
				 @DollarAmount
				,@ForeignKey
				,@IsYes
				,@Label
				,@LuckyNumber
				,@MathCalculation
				,@ReferenceId
				,@RightNow)

			 SELECT SCOPE_IDENTITY() AS PK;";

			var lst = GetParameters(entity);

			using (var dr = ExecuteReaderText(sql, lst.ToArray()))
			{
				//dr.Read();

				return Convert.ToInt32(GetScalar(dr, "PK"));
			}
		}

		public void Update(RudimentaryEntity entity)
		{
			var sql = @"UPDATE dbo.RudimentaryEntity SET 
						 DollarAmount = @DollarAmount
						,ForeignKey = @ForeignKey
						,IsYes = @IsYes
						,Label = @Label
						,LuckyNumber = @LuckyNumber
						,MathCalculation = @MathCalculation
						,ReferenceId = @ReferenceId
						,RightNow = @RightNow
					WHERE PrimaryKey = @PrimaryKey";

			var lst = GetParameters(entity);

			var p = GetPrimaryKeyParameter(entity.PrimaryKey);

			lst.Add(p);

			ExecuteNonQuery(sql, lst.ToArray());
		}

		private SqlParameter GetPrimaryKeyParameter(int primaryKey)
		{
			var p = new SqlParameter();
			p.ParameterName = "@PrimaryKey";
			p.SqlDbType = SqlDbType.Int;
			p.Value = primaryKey;

			return p;
		}

		private List<SqlParameter> GetParameters(RudimentaryEntity entity)
		{
			SqlParameter p = null;

			var lst = new List<SqlParameter>();

			p = new SqlParameter();
			p.ParameterName = "@DollarAmount";
			p.SqlDbType = SqlDbType.Decimal;
			p.Value = entity.DollarAmount;

			lst.Add(p);

			p = new SqlParameter();
			p.ParameterName = "@ForeignKey";
			p.SqlDbType = SqlDbType.Int;
			p.Value = entity.ForeignKey;

			lst.Add(p);

			p = new SqlParameter();
			p.ParameterName = "@IsYes";
			p.SqlDbType = SqlDbType.Bit;
			p.Value = entity.IsYes;

			lst.Add(p);

			p = new SqlParameter();
			p.ParameterName = "@Label";
			p.SqlDbType = SqlDbType.VarChar;
			p.Value = entity.Label;
			p.Size = 50; //This is made up

			lst.Add(p);

			p = new SqlParameter();
			p.ParameterName = "@LuckyNumber";
			p.SqlDbType = SqlDbType.Int;
			p.Value = entity.LuckyNumber;

			lst.Add(p);

			p = new SqlParameter();
			p.ParameterName = "@MathCalculation";
			p.SqlDbType = SqlDbType.Float;
			p.Value = entity.MathCalculation;
			p.Size = 53; //FLOAT(53) is supposed to be equivalent of Double

			lst.Add(p);

			p = new SqlParameter();
			p.ParameterName = "@ReferenceId";
			p.SqlDbType = SqlDbType.UniqueIdentifier;
			p.Value = entity.ReferenceId;

			lst.Add(p);

			p = new SqlParameter();
			p.ParameterName = "@RightNow";
			p.SqlDbType = SqlDbType.DateTime2;
			p.Value = entity.RightNow;
			p.Size = 0; //DATETIME2(0)

			lst.Add(p);

			return lst;
		}

		private RudimentaryEntity ToEntity(IDataReader reader)
		{
			var r = reader;

			var e = new RudimentaryEntity();
			e.PrimaryKey = Convert.ToInt32(r["PrimaryKey"]);
			e.DollarAmount = Convert.ToDecimal(r["DollarAmount"]);
			e.ForeignKey = Convert.ToInt32(r["ForeignKey"]);
			e.IsYes = Convert.ToBoolean(r["IsYes"]);
			e.Label = Convert.ToString(r["Label"]);
			e.LuckyNumber = Convert.ToInt32(r["LuckyNumber"]);
			e.MathCalculation = Convert.ToDouble(r["MathCalculation"]);
			e.ReferenceId = Guid.Parse(Convert.ToString(r["ReferenceId"]));
			e.RightNow = Convert.ToDateTime(r["RightNow"]);

			return e;
		}
	}
}
