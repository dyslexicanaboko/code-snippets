<Query Kind="Program">
  <NuGetReference>Newtonsoft.Json</NuGetReference>
  <Namespace>Newtonsoft.Json</Namespace>
  <Namespace>Newtonsoft.Json.Linq</Namespace>
</Query>

//Prototyping for a more built out mechanism. I am going to try to generalize it later.
void Main()
{
	
}

private JObject _json;

private void TestLoadObjects()
{
	var arr = new[]
	{
		//Whole value - no ArchiveMethod
		"{\"Client\":\"ClientA,ClientB\",\"ImportPath\":\"C:\\\\DirA\\\\\",\"FilePatterns\":[{\"FtpServersId\":4,\"RemotePath\":\"RemotePath/\",\"FilePattern\":\"File*.txt\",\"IsRequired\":true,\"CompressionFolderName\":\"FolderA\"},{\"FtpServersId\":4,\"FilePattern\":\"FileB*.txt\",\"IsRequired\":false,\"RemotePath\":\"RemotePath/\",\"CompressionFolderName\":\"FolderB\"},{\"FtpServersId\":4,\"FilePattern\":\"FileC*.txt\",\"IsRequired\":false,\"RemotePath\":\"RemotePath/\",\"CompressionFolderName\":\"Folder\"}],\"ArchivePolicy\":{\"FtpServersId\":\"4\",\"RemotePath\":\"/archivePath/\",\"IsCompressed\":true,\"CompressionMode\":\"filepattern\",\"CompressionFolderName\":\"System.String\"},\"NotificationPreferences\":{\"NotifyOnSuccess\":true,\"ContactList\":[{\"Email\":\"e1@e1.com\"},{\"Email\":\"e2@e2.com\"},{\"Email\":\"e3@e3.com\"},{\"Email\":\"e4@e4.com\"}]}}",
		//Archive Policy object only
		//"{\"ArchivePolicy\":{\"FtpServersId\":\"4\",\"RemotePath\":\"/archivePath/\",\"IsCompressed\":true,\"CompressionMode\":\"filepattern\",\"CompressionFolderName\":\"System.String\"}}",
		//Archive Policy missing values
		//"{\"ArchivePolicy\":{\"ArchiveMethod\":\"UNC\",\"IsCompressed\":true,\"CompressionMode\":\"filepattern\",\"UncPath\":\"C:\\\\BLah\\\\blah\\\\\"}}"
	};

	foreach (var j in arr)
	{
		_json = JObject.Parse(j);

		GetArchivePolicy();

		GetDivision();

		GetImportPath();

		GetImportFiles();
	}
}

public void GetArchivePolicy()
{
	var value = _json.GetValue("ArchivePolicy");

	var a = JsonConvert.DeserializeObject<ArchivePolicy>(value.ToString());

	a.Dump();
}

public void GetImportFiles()
{
	var value = _json.GetValue("FilePatterns");

	var a = JsonConvert.DeserializeObject<ImportFile[]>(value.ToString());

	a.Dump();
}

public void GetDivision()
{
	var value = _json.GetValue("Division");

	var a = value.ToString();

	a.Dump();
}

public void GetImportPath()
{
	var value = _json.GetValue("ImportPath");

	var a = Convert.ToString(value);

	a.Dump();
}

// Define other methods and classes here
public class ArchivePolicy
{
	public string ArchiveMethod { get; set; }

	public int FtpServersId { get; set; }

	public string RemotePath { get; set; }

	public bool IsCompressed { get; set; }

	public string CompressionMode { get; set; }

	public string CompressionFolderName { get; set; }
	
	public string UncPath { get; set; }
}

public class ImportFile
{
	public int FtpServersId { get; set; }

	public string RemotePath { get; set; }

	public string FilePattern { get; set; }

	public bool IsRequired { get; set; }

	public string CompressionFolderName { get; set; }
	
	public string CompressionMode { get; set; }
}