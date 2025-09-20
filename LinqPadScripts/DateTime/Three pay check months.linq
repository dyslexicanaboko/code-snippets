<Query Kind="Program">
  <Namespace>System.Globalization</Namespace>
</Query>

void Main()
{
	// These are Fridays
	// 2024-01-12
	// 2025-01-10

	//ProjectPayDays("2024-01-12").Dump();
	GetThreePayCheckMonths("2024-01-12");
	GetThreePayCheckMonths("2025-01-10");
}

public void GetThreePayCheckMonths(string firstFriday)
{
	var dtm = TryParseDateTimeString(firstFriday, nameof(firstFriday));
	
	var lst = ProjectPayDays(dtm);

	var culture = new CultureInfo("en-US");
	
	var months = lst
		.Select(x => x.Month)
		.GroupBy(month => month)
		.Select(x => new { Key = x.Key, Count = x.Count() })
		.Where(x => x.Count > 2)
		.Select(x => x.Key)
		.ToList();
		
	foreach (var m in months)
	{
		Console.WriteLine($"{dtm.Year} - {culture.DateTimeFormat.GetMonthName(m)}");

		var dates = lst
			.Where(d => d.Month == m)
			.Select(d => d.ToString("yyyy-MM-dd"))
			.ToList();

		dates.ForEach(Console.WriteLine);
	
		Console.WriteLine();
	}
}

//52 weeks, divided by 2 = 26 pay days in a 52 week year
private const int PaymentsPerYear = 26;
//Since it's two weeks between pay days, it would be a 14 day increment
private const int PayPeriodInDays = 14;

public List<DateTime> ProjectPayDays(DateTime firstFriday)
{
	//Kill the time component
	var s = firstFriday.Date;

	var lst = new List<DateTime>(PaymentsPerYear)
	{
		firstFriday
	};
	
	for (var i = 1; i < PaymentsPerYear; i++)
	{
		s = s.AddDays(PayPeriodInDays);
		
		//Getting paid every other week, which is two week intervals
		lst.Add(s);
	}

	return lst;
}

public DateTime TryParseDateTimeString(string dateTimeString, string label)
{
	if (!DateTime.TryParse(dateTimeString, out var dtm))
		throw new Exception($"{label} date is using an invalid format: {dateTimeString}");

	return dtm;
}
