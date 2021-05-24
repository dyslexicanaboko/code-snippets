<Query Kind="Program" />

void Main()
{
	//GetBeginningOfTheWeekDate();
	
	GetDay();
}

public void GetBeginningOfTheWeekDate()
{
	var dtm = new DateTime(2020, 2, 9);

	var dn = Convert.ToInt32(dtm.DayOfWeek);

	var sunday = dtm.AddDays(-1 * dn);

	sunday.Dump();
}

public void GetDay()
{
	var str = GetDay(new DateTime(2019, 12, 31), 6);
	
	str.Dump();
}

public string GetDay(DateTime start, int offset)
{
	var d = start.AddDays(offset);

	return d.Day.ToString();
}