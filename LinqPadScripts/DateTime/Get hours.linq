<Query Kind="Program" />

void Main()
{
	Console.WriteLine("Single");
	Single();
	
	Console.WriteLine("\nMultiple");
	Multiple();
}

private void Single()
{
	GetHours(DateTime.Today, "9:45", "10:30").Dump();
}

private void Multiple()
{
	var lst = new List<TimeBlock>()
	{
		new TimeBlock() { Start = "9:45", End = "10:30" },
		new TimeBlock() { Start = "15:40", End = "18:06" },
		new TimeBlock() { Start = "22:00", End = "23:00" }
	};
	
	GetHours(DateTime.Today, lst).Dump();

//	lst = new List<TimeBlock>()
//	{
//		new TimeBlock() { Start = "13:30", End = "14:38" },
//		new TimeBlock() { Start = "16:30", End = "17:45" }
//	};
//
//	GetHours(DateTime.Today, lst).Dump();
}

public class TimeBlock
{
	public string Start { get; set; }
	public string End { get; set; }
}

private double GetHours(DateTime date, string start, string end)
{
	var lst = new List<TimeBlock>()
	{
		new TimeBlock() { Start = start, End = end }
	};
	
	return GetHours(date, lst);
}

// Define other methods and classes here
private double GetHours(DateTime date, List<TimeBlock> blocks)
{
	double dblHours = 0;
	string strDate = date.ToString("yyyy-MM-dd");
	DateTime dtmStart;
	DateTime dtmEnd;
	
	foreach(TimeBlock tb in blocks)
	{
		dtmStart = Convert.ToDateTime(strDate + " " + tb.Start);
		dtmEnd = Convert.ToDateTime(strDate + " " + tb.End);
		
		if(dtmEnd < dtmStart)
			dtmEnd = dtmEnd.AddDays(1);
		
		dblHours += (dtmEnd - dtmStart).TotalHours;
	}
	
	return dblHours;
}