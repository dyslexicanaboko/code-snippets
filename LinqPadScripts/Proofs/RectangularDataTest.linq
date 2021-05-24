<Query Kind="Program" />

void Main()
{
	var dt = GetDataTable();
	
	dt.Dump();
	
	GetRectangles(dt);
}

public void GetRectangles(DataTable dt)
{
	var lst = new List<Outside>();
	
	foreach(DataRow dr in dt.Rows)
	{
		var obj = new Outside() 
		{ 
			T1Pk = Convert.ToInt32(dr["T1Pk"]),
			T1Col0 = Convert.ToString(dr["T1Col0"]) 
		};
		
		//Attempt group by syntax later
		obj.InsideRows = GetInsideRectangle(dt, "T2Fk", obj.T1Pk);
		
		lst.Add(obj);
	}
}

public List<Inside> GetInsideRectangle(DataTable dt, string columnName, int primaryKey)
{
	return null;
}

// Define other methods and classes here
public DataTable GetDataTable()
{
	var dt = new DataTable();
	dt.Columns.Add("T1Pk", typeof(int));
	dt.Columns.Add("T1Col0");
	dt.Columns.Add("T2Pk", typeof(int));
	dt.Columns.Add("T2Fk", typeof(int));
	dt.Columns.Add("T2Col0");
	
	var k = 1;
	
	for(var i = 1; i <= 10; i++)
	{
		for(var j = 1; j <= 10; j++)
		{
			var dr = dt.NewRow();
	
			dr["T1Pk"] = i;
			dr["T1Col0"] = "Outside";
			dr["T2Pk"] = k++;
			dr["T2Fk"] = i;
			dr["T2Col0"] = "Inside";
	
			dt.Rows.Add(dr);
		}
	}
	
	return dt;
}

public class Outside
{
	public int T1Pk { get; set; }
	public string T1Col0 { get; set; }
	public List<Inside> InsideRows { get; set; } = new List<Inside>();
}

public class Inside
{
	public int T2Pk { get; set; }
	public int T2Fk { get; set; }
	public string T2Col0 { get; set; }
}