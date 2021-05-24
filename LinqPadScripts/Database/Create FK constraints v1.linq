<Query Kind="Program" />

void Main()
{
	CreateForeignKeyConstraints("dbo", "TableA", "dbo", "TableB").Dump();
}

//Use if both tables share the same schema
public string CreateForeignKeyConstraints(string schema, string targetTable, string foreignTable)
{
	return CreateForeignKeyConstraints(schema, targetTable, schema, foreignTable);
}

//Use if tables are in different schemas
//Assumes you are using the same column name for the foreign key column
public string CreateForeignKeyConstraints(string targetTableSchema, string targetTable, string foreignTableSchema, string foreignTable)
{
	var fkIdPrefix = foreignTable;
	
	//CONSTRAINT [FK_Jobs.Schedules_Jobs.OccurrenceUnits_OccurrenceUnitsId] FOREIGN KEY ([OccurrenceUnitsId]) REFERENCES [Jobs].[OccurrenceUnits] ([OccurrenceUnitsId])
	var str = $"CONSTRAINT [FK_{targetTableSchema}.{targetTable}_{foreignTableSchema}.{foreignTable}_{foreignTable}Id] FOREIGN KEY ([{foreignTable}Id]) REFERENCES [{foreignTableSchema}].[{foreignTable}] ([{foreignTable}Id])";
	
	return str;
}