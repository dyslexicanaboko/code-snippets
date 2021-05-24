<Query Kind="Program" />

void Main()
{
	string strPath = @"C:\Dev\ProjectA\bin\";
	
	List<LibVersion> lst = 
		new DirectoryInfo(strPath)
		.EnumerateFiles("*.dll", SearchOption.TopDirectoryOnly)
		.Select(x => new LibVersion(x))
		.ToList();
		
	lst.Dump();	
}

public static string GetAssemblyAttribute<T>(Assembly asm, Func<T, string> value)
	where T : Attribute
{
	T attribute = (T)Attribute.GetCustomAttribute(asm, typeof(T));

	return attribute == null ? null : value.Invoke(attribute);
}

// Define other methods and classes here
public class LibVersion
{ 
	public LibVersion(FileInfo fileInfo)
	{
		FileInfoRef = fileInfo;
		
		FileName = fileInfo.Name;
		ModifiedDate = fileInfo.LastWriteTime;
		SizeInKb = fileInfo.Length / 1024;

		AssemblyRef = Assembly.LoadFile(fileInfo.FullName);

		var a = AssemblyRef;

		Product = GetAssemblyAttribute<AssemblyProductAttribute>(a, x => x.Product);
		Company = GetAssemblyAttribute<AssemblyCompanyAttribute>(a, x => x.Company);
		FullName = a.FullName;
		VersionNumber = a.GetName().Version.ToString();
		RuntimeVersion = a.ImageRuntimeVersion;
	}

	public FileInfo FileInfoRef { get; set; }

	public Assembly AssemblyRef { get; set; }

	public string Product { get; set; }
	
	public string Company { get; set; }

	public string FileName { get; set; }
	
	public string FullName { get; set; }

	public string VersionNumber { get; set; }

	public string RuntimeVersion { get; set; }

	public long SizeInKb { get; set; }

	public DateTime ModifiedDate { get; set; }
}