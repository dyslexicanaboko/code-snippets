<Query Kind="Program" />

void Main()
{
	var dt = FlatFileToDataTable(@"F:\OneDrive\Home\House\2902 North 33rd Terrace\Association\Board\2018\Financials\AvidPay CSV with Invoices.csv", true);
	
	dt.Dump();
}

// Define other methods and classes here
public DataTable FlatFileToDataTable(string fullFilePath, bool hasHeader)
{
	var dt = new DataTable("Table1");

	var lines = File.ReadAllLines(fullFilePath);
	
	var row0Arr = lines[0].Split(',');

	Func<int, string> GetColumnName = null;
	
	if(hasHeader)
	{
		GetColumnName = (int index) => { return row0Arr[index]; };
	}
	else
	{
		GetColumnName = (int index) => { return "Col_" + index; };
	}
	
	//Create the columns
	for (int i = 0; i < row0Arr.Length; i++)
	{
		var dc = new DataColumn(GetColumnName(i));
		
		dt.Columns.Add(dc);
	}
	
	//Handle the first row separately
	var j = 0;
	string[] tokens;

	if (hasHeader)
	{
		j = 1;
		tokens = lines[j].Split(',');
	}
	else
	{
		j = 0;
		tokens = row0Arr;
	}
	
	PopulateDataTable(dt, tokens);
	
	var data = lines.Skip(j + 1).ToArray();
	
	for (; j < data.Length; j++)
	{
		tokens = data[j].Split(',');
		
		PopulateDataTable(dt, tokens);
	}
	
	return dt;
}

//I need to handle text qualified data
private string SplitF(string line)
{
	var arr = line.Split(',', );
	
}

private void PopulateDataTable(DataTable dt, string[] columns)
{
	try
	{	        
		var dr = dt.NewRow();
		
		for (int i = 0; i < columns.Length; i++)
		{
			var c = columns[i];
			
			dr[i] = c;
		}
		
		dt.Rows.Add(dr);
	}
	catch (Exception ex)
	{
		
		throw;
	}
}