<Query Kind="Program" />

void Main()
{
	//UTC-5   = EST (Florida)
	//UTC-6   = //CST, CDT
	//UTC+5.5 = India
	//	TimeInIndia().Dump();
	//	MeetingTimes(DateTime.Parse("2017-1-9 23:00"), 5.5); //EST, EDT
	//	MeetingTimes(DateTime.Parse("2017-1-10 00:15"), -6); //CST, CDT
	//TimeFromToday(new DateTime(2016, 4, 05, 0, 0, 0)).Dump();
	//CountMonths(new DateTime(2016, 4, 05, 0, 0, 0)).Dump();
	//GiveMeAGuidString().Dump();
	//GenerateDummyString(128).Dump();

	//GenerateClientUniqueIds(26).Dump();
	//GetStringLength(@"C:\Dev\nadcwpapptfs01\PWSCollection\Facility Scheduler\Development\Services provided to Isas\Fs.Services.ServiceContracts");

	//SplitString(@"207|60|8340|20700072|BrittoB1|Britton, Robert, W|5082 Garden Way||Fremont|CA|94536||||||||||||BrittoB1@sutterhealth.org||M|S|1000000|N|12232012|03051955||10091976||AA||||06|20260010||CC-TRN||01222008|JCAHO-COMP||02052008|||||||||||||||||||||||||EDL=80.000000,ESL=194.212000,PTO=270.540000||||||Aide-Diet I|SEIU|SEIU|EDEN 250|EMC - Local 250|1", "|");

	//This is for tSQL format
	//Console.WriteLine($"0x{ByteArrayToString(0, 0, 0, 0, 44, 33, 146, 137)}");

	//DaysAgoFromToday("12/02/2020").Dump();
	SubtractDates("2020-11-13", "2021-01-28").Dump();
	//(t.TotalMinutes + 30).Dump();
	//DateTime.Parse("2019-01-13").AddDays(35).Dump();
	
}

public static void CombineTextFiles(bool hasHeader, string path, string filePattern, string saveAs)
{
	List<string> lst = new DirectoryInfo(path).GetFiles(filePattern).Select(x => x.FullName).ToList();
	
	CombineTextFiles(hasHeader, lst, saveAs);
}

public static void CombineTextFiles(bool hasHeader, IList<string> paths, string saveAs)
{
	var lstArr = new List<string[]>();

	string[] arr = null;
	
	int intSize = 0;

	int intHeader = hasHeader ? 1 : 0;

	foreach (string path in paths)
	{
		arr = File.ReadAllLines(path);
		
		intSize += arr.Length - intHeader;
		
		lstArr.Add(arr);
	}

	var lst = new List<string>(intSize);

	foreach(string[] a in lstArr)
		lst.AddRange(a.Skip(intHeader));
	
	lst.Sort();

	if (hasHeader)
		lst.Insert(0, lstArr[0][0]);

	File.WriteAllLines(saveAs, lst);

	Console.WriteLine($"{lst.Count} lines saved to file:\n{saveAs}");
}

//http://stackoverflow.com/questions/311165/how-do-you-convert-byte-array-to-hexadecimal-string-and-vice-versa
public static string ByteArrayToString(params byte[] ba)
{
	var hex = new StringBuilder(ba.Length * 2);
	
	foreach (byte b in ba)
		hex.AppendFormat("{0:x2}", b);
	
	//This only returns the hex string and DOES NOT prefix it with 0x
	return hex.ToString();
}

//http://stackoverflow.com/questions/311165/how-do-you-convert-byte-array-to-hexadecimal-string-and-vice-versa
public static byte[] StringToByteArray(string hex)
{
	int NumberChars = hex.Length;
	byte[] bytes = new byte[NumberChars / 2];
	for (int i = 0; i < NumberChars; i += 2)
		bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
	return bytes;
}

// Define other methods and classes here
private static string GenerateDummyString(int characters)
{
	return string.Empty.PadLeft(characters, 'a');
}

// Define other methods and classes here
private Guid GiveMeAGuid()
{
	return Guid.NewGuid();
}

//Format strings
//https://msdn.microsoft.com/en-us/library/97af8hh4(v=vs.110).aspx
private object GiveMeAGuidString(string format = null)
{
	string g = GiveMeAGuid().ToString(format);

	return new 
	{ 
		GuidUpper = g.ToUpper(), 
		GuidLower = g.ToLower() 
	};
}

private List<string> GenerateClientUniqueIds(int numberToGenerate)
{
	var lst = new List<string>();
	string g = null;
	
	for (int i = 0; i < numberToGenerate; i++)
	{
		g = "DIV_" + GiveMeAGuid().ToString().Substring(0, 3);
		
		if(!lst.Contains(g))
			lst.Add(g);
		else
			i--;
	}
	
	return lst;
}

private static string GetCurrentDirectory()
{
	return Path.GetDirectoryName(Util.CurrentQueryPath);
}

public void GetStringLength(string input)
{
	input.Length.Dump();
}

public void SplitString(string input, params string[] delimiters)
{
	var lst = new List<Tuple<int, string>>();
	
	string[] arr = input.Split(delimiters, StringSplitOptions.None);
	
	for(int i = 0; i < arr.Length; i++)
		lst.Add(new Tuple<int, string>(i, arr[i]));
		
	lst.Dump();
}

public TimeSpan SubtractDates(string startDate, string endDate)
{
	var dtmStart = DateTime.Parse(startDate);
	var dtmEnd = DateTime.Parse(endDate);

	return SubtractDates(dtmStart, dtmEnd);
}

public TimeSpan SubtractDates(DateTime startDate, DateTime endDate)
{
	var ts = endDate - startDate;
	
	Console.WriteLine($"Days {ts.Days}");
	
	return endDate - startDate;
}

public TimeSpan TimeFromToday(DateTime oldDate)
{
	return DateTime.Now - oldDate;
}

public double DaysAgoFromToday(string oldDate)
{
	return DaysAgoFromToday(DateTime.Parse(oldDate));
}

public double DaysAgoFromToday(DateTime oldDate)
{
	return TimeFromToday(oldDate).TotalDays;
}

public int CountMonths(DateTime start, DateTime? end = null)
{
	DateTime e = end.HasValue ? end.Value : DateTime.Today;

	DateTime s = start;
	
	int months = 0;

	while (s < e)
	{
		months++;

		Console.WriteLine($"Month {months} Start: {s} End: {e}");
		
		s = s.AddMonths(1);
	}
	
	int lastDayOfMonth = LastDayOfTheMonth(e);
	
	if((lastDayOfMonth - e.Day) > 5)
		months--;
		
	return months;
}

public int LastDayOfTheMonth(DateTime target)
{
	return new DateTime(target.Year, target.Month + 1, 1, 0, 0, 0).AddDays(-1).Day;
}

public void MeetingTimes(DateTime meetingLocalTime, double participantHourOffset)
{
	DateTime d = GetUtcTime(participantHourOffset, meetingLocalTime);
	
	Console.WriteLine($"Local meeting time: {meetingLocalTime}");
	Console.WriteLine($"Participant meeting time: {d}");
}

public DateTime TimeInIndia()
{
	return GetUtcTime(5.5);
}

public DateTime GetUtcTime(double hourOffset = 0, DateTime? target = null)
{
	DateTime d = target.HasValue ? target.Value.ToUniversalTime() : DateTime.UtcNow;

	return d.AddHours(hourOffset);
}