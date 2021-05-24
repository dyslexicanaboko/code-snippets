SELECT
	   s.name AS SchemaName
	  ,t.name AS TableName
	  ,'[' + s.name + '].[' + t.name + ']' AS QualifiedSchemaAndTable
FROM sys.tables t
	INNER JOIN sys.schemas s
		ON t.schema_id = s.schema_id
WHERE t.name <> '__MigrationHistory'