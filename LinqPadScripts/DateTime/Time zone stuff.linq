<Query Kind="Statements" />

var dtm = DateTime.Now;

var tz = TimeZoneInfo.Local;

tz.Dump();

var isDst = tz.IsDaylightSavingTime(dtm);

Console.WriteLine($"Is Daylight Saving Time : {isDst}");

var strTimeZone = isDst ? tz.DaylightName : tz.StandardName;

Console.ForegroundColor = ConsoleColor.Yellow;
Console.Write(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss zzz"));
Console.ResetColor();
Console.Write(" @ ");
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine(" " + strTimeZone);
Console.WriteLine();
Console.ResetColor();