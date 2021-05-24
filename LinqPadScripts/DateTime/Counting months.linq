<Query Kind="Program" />

void Main()
{
	CountMonthsAndPrint("2018-04-01", "2018-04-01");
	CountMonthsAndPrint("2018-04-01", "2018-04-02");
	CountMonthsAndPrint("2018-04-01", "2018-04-30");
	CountMonthsAndPrint("2018-04-01", "2018-05-01");
	CountMonthsAndPrint("2018-04-01", "2018-06-01");
	CountMonthsAndPrint("2018-04-01", "2018-06-15");
}

public void CountMonthsAndPrint(string start, string end)
{
	var m = CountMonthsBetweenDates(start, end);

	Console.WriteLine($"{start} to {end} = {m} months");
}

public int CountMonthsBetweenDates(string start, string end)
{
	var dtmStart = TryParseDateTimeString(start, "Start");
	var dtmEnd = TryParseDateTimeString(end, "End");

	return CountMonthsBetweenDates(dtmStart, dtmEnd);
}

public int CountMonthsBetweenDates(DateTime start, DateTime end)
{
	//Kill the time component
	var s = start.Date;
	var e = end.Date;

	var i = 0;

	for (var d = s; d < e; d = d.AddMonths(1))
	{
		i++;
	}

	return i;
}

public DateTime TryParseDateTimeString(string dateTimeString, string label)
{
	if (!DateTime.TryParse(dateTimeString, out var dtm))
		throw new Exception($"{label} date is using an invalid format: {dateTimeString}");

	return dtm;
}