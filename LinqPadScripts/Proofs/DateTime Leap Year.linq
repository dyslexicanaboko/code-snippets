<Query Kind="Program" />

void Main()
{
	DateTime.Now.ToString().Dump();
	var ts = (DateTime.Now - DateTime.Now.AddDays(-400));
	ts.ToString().Dump();
	
	DateTime.Parse("2024-01-31 23:59:59.999").AddMonths(1).Dump();
	
	(31 - 29).Dump();

	DateTime.Parse("2023-12-31 23:59:59.999").AddHours(1).Dump();

	ListLastDaysOfEachMonth(2023);
	
	Console.WriteLine();
	
	ListLastDaysOfEachMonth(2024);
}

// You can define other methods, fields, classes and namespaces here
private void ListLastDaysOfEachMonth(int year)
{
	var yearType = IsLeapYear(year) ? "Leap" : "Normal";

	Console.WriteLine($"{yearType} year: {year}");
	
	for (int i = 1; i <= 12; i++)
	{
		Console.WriteLine($"{i:00} -> {DateTime.DaysInMonth(year, i)}");
	}
}

private bool IsLeapYear(int year)
	=> (year % 4 == 0 && year % 100 != 0) || year % 400 == 0;
