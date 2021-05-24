<Query Kind="Program" />

void Main()
{
	var lst = new List<Profile>
	{
		new CsvProfile
		{
			Label = "FileA.txt",
			RowExample = "A,B,C",
			UseRowExample = true
		},
		new PipeProfile
		{
			Label = "FileB.txt",
			FullFilePath = @"C:\FileB.txt"
		},
		new PipeProfile
		{
			Label = "FileC.txt",
			RowExample = "A|B|C|D",
			UseRowExample = true
		}
	};

	lst.ForEach(Visualize);
}

//This doesn't handled text qualified yet
public void Visualize(Profile profile)
{
	string data = null;

	if (profile.UseRowExample)
	{
		data = profile.RowExample;
	}
	else
	{
		data = File.ReadAllText(profile.FullFilePath);
	}

	var D = profile.Delimiter;

	var q = data.Split('\n').Where(x => !string.IsNullOrWhiteSpace(x));

	if (profile.Top.HasValue)
	{
		q = q.Take(profile.Top.Value);
	}

	var lines = q.ToArray();

	//Arbitrarily choose the first line to get the width of the file
	var cols = lines[0].Split(D).Length;

	var dt = new DataTable();

	//Construct the schema of the table, all string for max compatability
	for (int i = 0; i < cols; i++)
	{
		//Add the index as the column name
		dt.Columns.Add(i.ToString(), typeof(string));
	}

	//For each row - no header
	for (int r = 0; r < lines.Length; r++)
	{
		var dr = dt.NewRow();
		var arr = lines[r].Split(D);

		for (int c = 0; c < dt.Columns.Count; c++)
		{
			if (c == arr.Length)
			{
				Console.WriteLine($"Problem on Row {r} and Column {c}");
			}
			
			dr[c] = arr[c];

//			if (arr[c] == "175743127")
//			{
//				if(true);
//			}
		}

		dt.Rows.Add(dr);
	}

	Console.WriteLine($"Label: {profile.Label}");
	Console.WriteLine($"Row count   : {dt.Rows.Count:n0}");
	Console.WriteLine($"Column count: {dt.Columns.Count:n0}\n");

	dt.Dump();
}

public class CsvProfile
	: Profile
{
	public CsvProfile()
	{
		Delimiter = ',';
	}
}

public class PipeProfile
	: Profile
{
	public PipeProfile()
	{
		Delimiter = '|';
	}
}

public class Profile
{
	public char Delimiter { get; set; }

	public string RowExample { get; set; }

	public bool UseRowExample { get; set; }

	public string FullFilePath { get; set; }
	
	public string Label { get; set; }

	public int? Top { get; set; } = null;
}