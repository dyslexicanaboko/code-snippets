<Query Kind="Program" />

void Main()
{
	var ts = (DateTime.Now - DateTime.Now.AddDays(-1));

	CalculatingParts(ts.TotalMilliseconds);

	Console.WriteLine();

	ts.TotalDays.Dump();
	ts.TotalHours.Dump();
	ts.TotalMinutes.Dump();
	ts.TotalSeconds.Dump();
	ts.TotalMilliseconds.Dump();

	Console.WriteLine();

	ts.Days.Dump();
	ts.Hours.Dump();
	ts.Minutes.Dump();
	ts.Seconds.Dump();
	ts.Milliseconds.Dump();
}

private void CalculatingParts(double totalMilliseconds)
{
	var totalSeconds = totalMilliseconds / 1000D;

	var days = totalSeconds / 86400D;
	var hours = totalSeconds / 3600D;
	var minutes = (totalSeconds % 3600D) / 60D;
	var seconds = (totalSeconds % 60D);
	var milliseconds = (totalMilliseconds % 1D) * 1000D;

	days.Dump();
	hours.Dump();
	minutes.Dump();
	seconds.Dump();
	milliseconds.Dump();

	GetWholeNumber(days).Dump();
	GetWholeNumber(hours).Dump();
	GetWholeNumber(minutes).Dump();
	GetWholeNumber(seconds).Dump();
	GetWholeNumber(milliseconds).Dump();
}

private int GetWholeNumber(double target) => Convert.ToInt32(Math.Round(target, 0, MidpointRounding.ToZero));