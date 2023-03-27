CREATE TYPE [dbo].[DateTime2List] AS TABLE
(
	DateTime2Value DateTime2(7) NOT NULL,
	UNIQUE CLUSTERED (DateTime2Value)
)