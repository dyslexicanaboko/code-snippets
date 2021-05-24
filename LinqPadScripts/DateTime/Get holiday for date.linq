<Query Kind="Program" />

void Main()
{
	GetHoliday(new DateTime(2019, 1, 2)).Dump();
}

private const string UnknownHoliday = "Other holiday or sabbatical";

private string GetHoliday(DateTime todaysDate)
{
	var dict = BuildHolidayList(todaysDate.Year);
	
	var d = todaysDate.Date;
	
	if(dict.ContainsKey(d)) return dict[d];
	
	return UnknownHoliday;
}

private int _year;

private Dictionary<DateTime, string> BuildHolidayList(int year)
{
	_year = year;

	var dict = new Dictionary<DateTime, string>
	{
		{ D(1, 1) , "New Year's Day" },
//No one is celebrating Inauguration Day - I didn't even know this was a thing
//		{ D(1, 20) , "Inauguration Day" },
//		{ D(1, 21) , "Inauguration Day" },
		{ D(7, 4) , "Independence Day" },
		{ D(11, 11) , "Veterans Day" },
		{ D(12, 25) , "Christmas" },
	};

	//Floating holidays
	// January 15 - 21
	FloatingHoliday(dict, 1, 15, 21, "Birthday of Martin Luther King, Jr.");
	
	// February 15 - 21
	FloatingHoliday(dict, 2, 15, 21, "Washington's Birthday");
	
	// May 25 - 31
	FloatingHoliday(dict, 5, 25, 31, "Memorial Day");
	
	// September 1 - 7
	FloatingHoliday(dict, 9, 1, 7, "Labor Day");
	
	// October 8 - 14
	FloatingHoliday(dict, 10, 8, 14, "Indigenous Peoples' Day");
	
	// November 22 - 28
	FloatingHoliday(dict, 11, 22, 28, "Thanksgiving Day");
	
	return dict;
}

private DateTime D(int month, int day)
{
	return new DateTime(_year, month, day);
}

private void FloatingHoliday(Dictionary<DateTime, string> dict, int month, int startDays, int endDays, string holiday)
{
	var f = D(month, startDays);

	int c = endDays - startDays + 1; //Add 1 to make it inclusive
	
	for (int i = 0; i < c; i++)
	{
		var d = f.AddDays(i);
		
		dict.Add(d, holiday);
	}	
}

