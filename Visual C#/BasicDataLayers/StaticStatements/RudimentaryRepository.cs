using BasicDataLayers.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace BasicDataLayers.StaticStatements
{
	public class RudimentaryRepository
		: BaseDal
	{
		public RudimentaryEntity Select(int primaryKey)
		{
			var sql = @"
			SELECT
				 DollarAmount
				,ForeignKey
				,IsYes
				,Label
				,LuckyNumber
				,MathCalculation
				,ReferenceId
				,RightNow
			FROM dbo.RudimentaryEntity
			WHERE PrimaryKey = @PrimaryKey";

			var p = new SqlParameter();
			p.ParameterName = "@PrimaryKey";
			p.SqlDbType = SqlDbType.Int;
			p.Value = primaryKey;

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

		private RudimentaryEntity ToEntity(IDataReader reader)
		{
			var r = reader;

			var e = new RudimentaryEntity();
			e.PrimaryKey = Convert.ToInt32(r[""]);
			e.DollarAmount = Convert.ToDecimal(r[""]);
			e.ForeignKey = Convert.ToInt32(r[""]);
			e.IsYes = Convert.ToBoolean(r[""]);
			e.Label = Convert.ToString(r[""]);
			e.LuckyNumber = Convert.ToInt32(r[""]);
			e.MathCalculation = Convert.ToDouble(r[""]);
			e.ReferenceId = Guid.Parse(Convert.ToString(r[""]));
			e.RightNow = Convert.ToDateTime(r[""]);

			return e;
		}

		public void Insert(RudimentaryEntity entity)
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
				,@RightNow)";

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
			
			ExecuteNonQuery(sql, lst.ToArray());
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

			p = new SqlParameter();
			p.ParameterName = "@PrimaryKey";
			p.SqlDbType = SqlDbType.Int;
			p.Value = entity.PrimaryKey;

			lst.Add(p);

			ExecuteNonQuery(sql, lst.ToArray());
		}
	}
}
