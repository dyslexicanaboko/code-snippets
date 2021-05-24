<Query Kind="Program" />

string BASE_PATH;
string TEMPLATES;
string SAVE_TO;

void Main()
{
	BASE_PATH = Path.GetDirectoryName(Util.CurrentQueryPath);
	TEMPLATES = Path.Combine(BASE_PATH, "Templates");
	SAVE_TO = Path.Combine(BASE_PATH, "Output");

	//Create the path if it doesn't exist
	if(!Directory.Exists(SAVE_TO)) Directory.CreateDirectory(SAVE_TO);

	List<Profile> lst = GetProfiles();

	GenerateBackupScript(lst);
}

private List<Profile> GetProfiles()
{
	var lst = new List<Profile>()
	{
		new Profile() { Database = "FacSchedCapital" },
		new Profile() { Database = "FacSchedCHE" },
		new Profile() { Database = "FacSchedEMHS" },
		new Profile() { Database = "FacSchedLifePoint" },
		new Profile() { Database = "FacSchedMLH" },
		new Profile() { Database = "FacSchedSanAntonio" },
		new Profile() { Database = "FacSchedWestFlorida" },
	};

	//The servers all have the same Bak location
	lst.ForEach(x =>
	{
		x.BackUpSetName = $"{x.Database}_v3.2_2018.01.24";
		x.SaveBakFileAsPath = @"G:\Workspace\Temp bak\" + x.BackUpSetName + ".bak";
	});

	return lst;
}

public class Profile
{
	public string Database { get; set; }
	public string BackUpSetName { get; set; }
	public string SaveBakFileAsPath { get; set; }
}

public void GenerateBackupScript(List<Profile> profiles)
{
	string strTemplate = File.ReadAllText(Path.Combine(TEMPLATES, "Template.sql"));

	var sb = new StringBuilder();

	foreach (var p in profiles)
	{
		sb.Append(strTemplate)
		  .Replace("{{DATABASE}}", p.Database)
		  .Replace("{{SAVE_BAK_AS_PATH}}", p.SaveBakFileAsPath)
		  .Replace("{{BACKUP_SET_NAME}}", p.BackUpSetName)
		  .AppendLine();

		Console.WriteLine($"Backup script appended for {p.Database} @ {DateTime.Now.ToString()}");
	}

	File.WriteAllText(Path.Combine(SAVE_TO, $"BackUp_{profiles.Count}_DB_{DateTime.Now.ToString("yyyy.MM.dd_HH.mm.ss")}.sql"), sb.ToString());
}