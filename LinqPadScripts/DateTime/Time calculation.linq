<Query Kind="Program" />

void Main()
{
	CalculateSpan("2019-04-06");
	//GetMonthCount("2018-03-01", "2018-02-01").Dump(); //Exception
	//GetMonthCount("2018-03-01", "2018-03-01").Dump();
	//GetMonthCount("2018-02-01", "2018-03-01").Dump();
	//GetXDaysFromToday(30);
}

public void GetXDaysFromToday(int days)
{
	GetXDaysFromDate(DateTime.Today, days);
}

public void GetXDaysFromDate(DateTime date, int days)
{
	var dtm = date.AddDays(days);
	
	Console.WriteLine(dtm.ToLongDateString());
}

// Define other methods and classes here
public void CalculateSpan(string dateTimeStart)
{
	var dtmStart = DateTime.Parse(dateTimeStart);
	var dtmEnd = DateTime.Today;
	
	CalculateSpan(dtmStart, dtmEnd);
}

public void CalculateSpan(string start, string end)
{
	var dtmStart = DateTime.Parse(start);
	var dtmEnd = DateTime.Parse(end);

	CalculateSpan(dtmStart, dtmEnd);
}

public void CalculateSpan(DateTime start, DateTime end)
{
	var ts = (end - start);
	var months = GetMonthCount(start, end);

	//ts.Dump();

	Console.WriteLine($"Start: {start}");
	Console.WriteLine($"End: {end}");
	Console.WriteLine($"Days: {ts.Days}");
	Console.WriteLine("Percent of year: {0:p2}", ((decimal)ts.Days / 365.0M));
	Console.WriteLine($"Weeks: {(ts.Days / 7)}");
	Console.WriteLine($"Months: {months}");
}

public int GetMonthCount(string start, string end)
{
	var s = DateTime.Parse(start);
	var e = DateTime.Parse(end);

	return GetMonthCount(s, e);
}

public int GetMonthCount(DateTime start, DateTime end)
{
	var dStart = FirstOfMonth(start);
	var dEnd = FirstOfMonth(end.Date);

	if(start == end) return 0;

	if (start > end)
		throw new Exception($"Start date \"{start.ToShortDateString()}\" cannot be greater than end date \"{end.ToShortDateString()}\"");
	
	var m = 0;
		
	for (var d = dStart; d < dEnd; d = d.AddMonths(1))
	{
		m++;
	}
	
	return m;
}

public DateTime FirstOfMonth(DateTime date)
{
	var d = date;
	
	var dtm = new DateTime(d.Year, d.Month, 1);
	
	return dtm;
}