<Query Kind="Program" />

void Main()
{
	var lines = Data.Split(new[] { Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries);
	var rows = new string[lines.Length,2];
	
	for (var r = 0; r < lines.Length; r++)
	{
		var l = lines[r];
		
		var arr = l.Split('\t');

		rows[r, 0] = arr[1];
		rows[r, 1] = arr[0];
	}
	
	rows.Dump();
}

// Data to parse
private const string Data = 
@"bigint	Int64	BigInt	GetSqlInt64	Int64	GetInt64
binary	Byte[]	VarBinary	GetSqlBinary	Binary	GetBytes
bit	Boolean	Bit	GetSqlBoolean	Boolean	GetBoolean
char	String	GetSqlString	AnsiStringFixedLength
date 1	DateTime	Date 1	GetSqlDateTime	Date 1	GetDateTime
datetime	DateTime	DateTime	GetSqlDateTime	DateTime	GetDateTime
datetime2	DateTime	DateTime2	None	DateTime2	GetDateTime
datetimeoffset	DateTimeOffset	DateTimeOffset	none	DateTimeOffset	GetDateTimeOffset
decimal	Decimal	Decimal	GetSqlDecimal	Decimal	GetDecimal
float	Double	Float	GetSqlDouble	Double	GetDouble
int	Int32	Int	GetSqlInt32	Int32	GetInt32
money	Decimal	Money	GetSqlMoney	Decimal	GetDecimal
nchar	String	NChar	GetSqlString	StringFixedLength	GetString
numeric	Decimal	Decimal	GetSqlDecimal	Decimal	GetDecimal
nvarchar	String	NVarChar	GetSqlString	String	GetString
real	Single	Real	GetSqlSingle	Single	GetFloat
rowversion	Byte[]	Timestamp	GetSqlBinary	Binary	GetBytes
smalldatetime	DateTime	DateTime	GetSqlDateTime	DateTime	GetDateTime
smallint	Int16	SmallInt	GetSqlInt16	Int16	GetInt16
smallmoney	Decimal	SmallMoney	GetSqlMoney	Decimal	GetDecimal
time	TimeSpan	Time	none	Time	GetDateTime
tinyint	Byte	TinyInt	GetSqlByte	Byte	GetByte
uniqueidentifier	Guid	UniqueIdentifier	GetSqlGuid	Guid	GetGuid
varbinary	Byte[]	VarBinary	GetSqlBinary	Binary	GetBytes
varchar	String	VarChar	GetSqlString	AnsiString, String	GetString
xml	Xml	Xml	GetSqlXml	Xml	none
";