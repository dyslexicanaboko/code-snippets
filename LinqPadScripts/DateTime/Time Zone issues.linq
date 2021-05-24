<Query Kind="Program" />

void Main()
{
	var tz = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
	var dtm = new DateTimeWithZone("2020-03-18 11:00:00", tz);

	//var tz = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
	//var dtm = new DateTimeWithZone("2020-04-15 19:00:00", tz);
	
	var l = dtm.LocalTime;
	var u = dtm.UniversalTime;
	
	var ts = u - l;

	l.Dump();
	u.Dump();
	ts.Dump();
}

// Define other methods and classes here
public struct DateTimeWithZone
{
	private readonly DateTime _utcDateTime;
	private readonly TimeZoneInfo _timeZone;

	public DateTimeWithZone(string dateTime, TimeZoneInfo timeZone)
		: this(Convert.ToDateTime(dateTime), timeZone)
	{
			
	}
	
	public DateTimeWithZone(DateTime dateTime, TimeZoneInfo timeZone)
	{
		var dateTimeUnspec = DateTime.SpecifyKind(dateTime, DateTimeKind.Unspecified);
		
		_utcDateTime = TimeZoneInfo.ConvertTimeToUtc(dateTimeUnspec, timeZone);
		
		_timeZone = timeZone;
	}

	public DateTime UniversalTime => _utcDateTime;

	public TimeZoneInfo TimeZone => _timeZone;

	public DateTime LocalTime => TimeZoneInfo.ConvertTime(_utcDateTime, _timeZone);
}