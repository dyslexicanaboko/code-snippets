<Query Kind="Program" />

string BasePath;

void Main()
{
	BasePath = @"J:\Tamerlane old docs\Vendors\";
	
	var arr = GetFiles(BasePath);
	
	//arr.Dump();
	
	//ReArrangeFileName("2007.06.14 John's Plumbing.pdf").Dump();
	
	arr.ForEach(x => x.RenameFile());
}

// Define other methods and classes here
public List<UpdatedFileNames> GetFiles(string path)
{
	var files = new DirectoryInfo(path)
	.GetFiles("*.pdf", SearchOption.TopDirectoryOnly)
	.Select(x => new UpdatedFileNames()
	{ 
		FileInfo = x,
		NewFileName = ReArrangeFileName(x.FullName)
	})
	.ToList();
	
	return files;
}

public string ReArrangeFileName(string fileName)
{
	fileName = Path.GetFileNameWithoutExtension(fileName);
	
	var date = fileName.Substring(0, 10);
	
	var subject = fileName.Substring(11, fileName.Length - 10 - 1);
	
	var newFileName = subject + " " + date + ".pdf";
	
	return newFileName;
}

public class UpdatedFileNames
{
	public FileInfo FileInfo { get; set; }

	public string NewFileName { get; set; }
	
	public void RenameFile()
	{
		File.Move(FileInfo.FullName, Path.Combine(FileInfo.DirectoryName, NewFileName));
	}
}

