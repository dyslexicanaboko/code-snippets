DECLARE @tbl TABLE
(
	TableName sysname NOT NULL,
	ColumnName sysname NOT NULL
);

INSERT INTO @tbl
(
    TableName
   ,ColumnName
)
SELECT
	   t.name AS TableName
      ,c.name AS ColumnName
FROM sys.schemas s
	INNER JOIN sys.tables t
		ON t.schema_id = s.schema_id
	INNER JOIN sys.columns c
		ON c.object_id = t.object_id
WHERE s.name = 'dbo'
	AND c.name LIKE '%Date%' -- Searching for any columns that have the word Date in them
	AND t.name NOT IN (
	-- Tables to ignore
	'TableA',
	'TableB',
	'TableC'
	)
	AND c.name NOT IN (
	-- Names of columns that have the word date in them but 
	-- are not part of Start and End like date columns
	'DateChangedUtc',
	'DateCreatedUtc',
	'CreatedDateUTC',
	'CreateDateUtc',
	'CreatedDate',
	'Date',
	'StatusDate',
	'DateUTC',
	'StatusDateUTC',
	'BirthDate',
	'SeniorityDate',
	'LastLoginDateUTC'
)
ORDER BY
	  t.name
	 ,c.name

--SELECT * FROM @tbl t

--RETURN

DECLARE @tblResults TABLE
(
	TableName sysname NOT NULL PRIMARY KEY,
	CountQuery VARCHAR(MAX) NOT NULL
);

DECLARE @tblTables TABLE
(
	EntryId INT IDENTITY(0, 1) NOT NULL PRIMARY KEY,
	TableName sysname NOT NULL
);

INSERT INTO @tblTables
(
    TableName
)
SELECT DISTINCT
	t.TableName
FROM @tbl t

--SELECT * FROM @tblTables

DECLARE @i INT = 0;
DECLARE @count INT = (SELECT COUNT(EntryId) FROM @tblTables);

/* Columns are referred to as Left and Right because the order
   of appearance is not guaranteed to be Start then End date.
   COUNT(*) being used because not all tables will have a PK 
   of some kind. */
WHILE @i < @count
BEGIN
	DECLARE @tableName sysname;

	SELECT @tableName = TableName FROM @tblTables WHERE EntryId = @i;
	
	INSERT INTO @tblResults
	(
	    TableName,
		CountQuery
	)
	SELECT 
		 x.TableName
		,CONCAT('SELECT ''', x.TableName, ''' AS TableName,'
			   ,'''', x.ColumnName, ''' AS LeftCol,'
			   ,'''', x.NextCol, ''' AS RightCol,'
			   ,'(SELECT COUNT(*) FROM dbo.', x.TableName, ' WHERE ', x.ColumnName, ' < ', x.NextCol, ') AS LessThan,'
		       ,'(SELECT COUNT(*) FROM dbo.', x.TableName, ' WHERE ', x.ColumnName, ' = ', x.NextCol, ') AS EqualTo,'
		       ,'(SELECT COUNT(*) FROM dbo.', x.TableName, ' WHERE ', x.ColumnName, ' > ', x.NextCol, ') AS GreaterThan'
			   ,' UNION ') AS CountQuery
	FROM (
		SELECT
			TableName
		   ,ColumnName
		   ,LAG(ColumnName, 1, NULL) OVER (ORDER BY ColumnName) AS NextCol
		FROM @tbl 
		WHERE TableName = @tableName
	) AS x
	WHERE x.NextCol IS NOT NULL

	SET @i += 1;
END

SELECT * FROM @tblResults tr
