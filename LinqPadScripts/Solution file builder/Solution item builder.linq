<Query Kind="Program" />

string BASE_PATH;
string SOLUTION_TEMPLATE_CSPROJ;
string SOLUTION_TEMPLATE_DTPROJ;

/* TODO
   Find a better way to handle the file extensions. */

void Main()
{
	BASE_PATH = Path.GetDirectoryName(Util.CurrentQueryPath);

	SOLUTION_TEMPLATE_CSPROJ = File.ReadAllText(Path.Combine(BASE_PATH, "SolutionTemplateCsProj.txt"));
	SOLUTION_TEMPLATE_DTPROJ = File.ReadAllText(Path.Combine(BASE_PATH, "SolutionTemplateDtProj.txt"));
	
	GenerateSolutionPerProject(DtProj, @"C:\Dev\nadcwpapptfs01\Git\ETL-Projects\FS\FS.ETL\");
	
	Console.WriteLine($"Finished @ {DateTime.Now.ToString()}");
}

private void GenerateOnSolutionMultipleProjects()
{
	var arr = GetProjectFileInfoFromFile("Projects.txt");
	
	GenerateSolutionForProject(CsProj, arr);
}

private void GenerateSolutionPerProject(string extension, string topLevelPath)
{
	var lst = GetProjectFileInfo(topLevelPath, extension);
	
	foreach (var p in lst)
	{
		Console.WriteLine(p.FullPath);
		
		var contents = GenerateSolutionForProject(extension, p);
		
		SaveAs(p, contents);
	}
}

private void SaveAs(Project project, string solutionContents)
{
	var path = Path.ChangeExtension(project.FullPath, @"sln");
	
	File.WriteAllText(path, solutionContents, Encoding.UTF8);
}

private string GuidAsString(Guid? guid = null)
{
	if(!guid.HasValue)
		guid = Guid.NewGuid();
	
	var str = guid.ToString().ToUpper();
	
	return str;
}

private string GenerateSolutionForProject(string extension, params Project[] projectsInSolution)
{
	var sb = new StringBuilder();
	var sbM = new StringBuilder();

	var guidSolution = GuidAsString();

	foreach (Project p in projectsInSolution)
	{
		var strPg = p.GuidId.ToString().ToUpper();

		var path = p.FullPath.Replace(p.Path, string.Empty);

		//Doing this for now, but I think this is wrong. I just don't remember why.
		if (path.StartsWith("\\"))
		{
			path = path.TrimStart('\\');
		}

		sb.Append("Project(\"{")
		  .Append(guidSolution)
		  .Append("}\") = \"")
		  .Append(p.Name.Replace(p.Extension, string.Empty))
		  .Append("\", \"")
		  .Append(path)
		  .Append("\", \"{")
		  .Append(strPg)
		  .AppendLine("}\"")
		  .AppendLine("EndProject");

		if(p.Extension == CsProj)
		{
			//C# projects
			sbM.Append("{").Append(strPg).Append("}.Debug|Any CPU.ActiveCfg = Debug|Any CPU").AppendLine()
			   .Append("{").Append(strPg).Append("}.Debug|Any CPU.Build.0 = Debug|Any CPU").AppendLine()
			   .Append("{").Append(strPg).Append("}.Release|Any CPU.ActiveCfg = Release|Any CPU").AppendLine()
			   .Append("{").Append(strPg).Append("}.Release|Any CPU.Build.0 = Release|Any CPU").AppendLine();
		}
		else
		{
			//SSIS projects
			sbM.Append("{").Append(strPg).Append("}.Development|Default.ActiveCfg = Development").AppendLine()
			   .Append("{").Append(strPg).Append("}.Development|Default.Build.0 = Development").AppendLine();
		}
	}

	var template = (extension == CsProj) ? SOLUTION_TEMPLATE_CSPROJ : SOLUTION_TEMPLATE_DTPROJ;

	var content = template
			.Replace("{{PROJECTS}}", sb.ToString())
		   	.Replace("{{PROJECT_MODES}}", sbM.ToString());
   
	return content;
}

private Project[] GetProjectFileInfoFromFile(string fileName)
{
	var lines = File.ReadAllLines(Path.Combine(BASE_PATH, fileName));
	
	var arr = new Project[lines.Length];
	
	for (int i = 0; i < lines.Length; i++)
	{
		var line = lines[i];
		
		var tokens = line.Split(',');

		arr[i] = new Project()
		{
			Name = tokens[0],
			Path = Path.GetDirectoryName(tokens[1]),
			FullPath = tokens[1],
			GuidId = new Guid(tokens[2]),
			Extension = Path.GetExtension(tokens[0])
		};
	}

	return arr;
}

private const string CsProj = ".csproj";
private const string DtProj = ".dtproj";

private Project[] GetProjectFileInfo(string topLevelPath, string extension)
{
	var files = new DirectoryInfo(topLevelPath).GetFiles($"*{extension}", SearchOption.AllDirectories);

	Func<FileInfo, Guid> f;

	if (extension == CsProj)
	{
		f = GetProjectGuid;
	}
	else
	{
		f = (FileInfo fi) => { return Guid.NewGuid(); };
	}

	var arr = new Project[files.Length];

	for (int i = 0; i < files.Length; i++)
	{
		var fi = files[i];
		
		arr[i] = new Project()
		{
			Name = fi.Name,
			Path = fi.DirectoryName,
			FullPath = fi.FullName,
			GuidId = f(fi),
			Extension = Path.GetExtension(fi.Name)
		};
	}
	
	return arr;
}

private Guid GetProjectGuid(FileInfo fi)
{
	var doc = new XmlDocument();
	doc.Load(fi.FullName);
	
	XmlNodeList lst = doc.GetElementsByTagName("ProjectGuid");
	
	if(lst.Count == 0)
		throw new ApplicationException("Guid not found in " + fi.FullName);
		
	return new Guid(lst[0].InnerText);
}

//Keeping this for later, this is for making something a project reference (vs. Dll reference)
private void ProjectReference()
{
	var sb = new StringBuilder();

	sb.AppendLine("<ItemGroup>")
	  .AppendLine("<ProjectReference Include=\"..\\{0}\">") //Path to csproj
	  .AppendLine("<Project>{1}</Project>") //Guid
	  .AppendLine("<Name>{2}</Name>") //Name
	  .AppendLine("</ProjectReference>")
	  .AppendLine("</ItemGroup>");
}

public class Project
{
	public string Name { get; set; }
	public string Path { get; set; }
	public string FullPath { get; set; }
	public string Extension { get; set; }
	public Guid GuidId { get; set; }
}
