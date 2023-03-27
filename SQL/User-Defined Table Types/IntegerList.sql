CREATE TYPE [dbo].[IntegerList] AS TABLE
(
	IntValue INT NOT NULL,
	UNIQUE CLUSTERED (IntValue)
)