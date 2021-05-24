<Query Kind="Program" />

void Main()
{
	//Generate a named foreign key constraint
	GenerateNamedForeignKeyConstraint("dbo.BillingCategory", "dbo.User", "UserId").Dump();
}

private string GenerateNamedForeignKeyConstraint(string thisTable, string otherTable, string sharedKey)
{
	//CONSTRAINT [FK_Jobs.JobDefinition_Jobs.JobCategory_JobCategoryId] FOREIGN KEY ([JobCategoryId]) REFERENCES [Jobs].[JobCategory] ([JobCategoryId]),
	string otherTableWrapped = null;
	
	var arr = otherTable.Split('.');

	if (arr.Length == 2)
	{
		for (int i = 0; i < arr.Length; i++)
		{
			var p = arr[i];
			
			if (!p.StartsWith("["))
			{
				p = "[" + p;
			}

			if (!p.EndsWith("]"))
			{
				p = p + "]";
			}
			
			arr[i] = p;
		}
		
		otherTableWrapped = string.Join(".", arr);
	}
	else
	{
		otherTableWrapped = otherTable;
	}
	
	var str = $"CONSTRAINT [FK_{thisTable}_{otherTable}_{sharedKey}] FOREIGN KEY ([{sharedKey}]) REFERENCES {otherTableWrapped} ([{sharedKey}]),";
	
	return str;
}