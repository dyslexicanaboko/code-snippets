<Query Kind="Program" />

const string TEMPLATE = "\"F:\\Program Files\\HandbrakeCmd\\HandBrakeCLI.exe\" input -i \"{0}\" -o \"{0}.mp4\" -O true -r 30 -a \"none\" -e \"x264\" -q 20";
//const string TEMPLATE = "\"F:\\Program Files\\HandbrakeCmd\\HandBrakeCLI.exe\" input -i \"{0}.mov\" -o \"{0}.mp4\" -O true -r 30 -a \"none\" -e \"x264\" -q 20";

void Main()
{
	string strPath = @"J:\Floridiots\Editing\Motorcycle douches\003\";

	//string[] arr = GetFiles(strPath);
	string[] arr = GetFilesOrderByDate(strPath);
	
	var sb = new StringBuilder();

	sb.AppendLine("ECHO OFF");
	sb.AppendLine("ECHO This batch file was generated using a script");
	sb.Append("ECHO ========= Script Path: ").AppendLine(Util.CurrentQueryPath);
	sb.Append("ECHO ========= Target Path: ").AppendLine(strPath);
	sb.AppendLine("ECHO .");
	sb.AppendLine("PAUSE");

	foreach (string s in arr)
		sb.AppendFormat(TEMPLATE, s).AppendLine();
	
	sb.AppendLine("ECHO .");
	sb.AppendLine("PAUSE");
		
	string str = sb.ToString();
	
	Console.WriteLine(str);
	
	File.WriteAllText(Path.Combine(strPath, "HandBrake Execution.bat"), str);
}

// Define other methods and classes here
public string[] GetFiles(string path)
{
	return Directory.GetFiles(path, "*.mov", SearchOption.TopDirectoryOnly)
					.Select(x => Path.GetFileNameWithoutExtension(x))
					.ToArray();
}

public string[] GetFilesOrderByDate(string path)
{
	return new DirectoryInfo(path)
				.GetFiles("*.mov", SearchOption.TopDirectoryOnly)
				.OrderByDescending(x => x.CreationTime)
				.Select(x => x.FullName)
				.ToArray();
}