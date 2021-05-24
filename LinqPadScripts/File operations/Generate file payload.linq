<Query Kind="Program" />

void Main()
{
	var size = GetFileSizeInBytes(Unit.MegaBytes, 1);
	
	CreateDummyFiles(1, size, @"J:\Dump\", "LargeGarbageFile", ".dat");
	
	Console.WriteLine($"Finished @ {DateTime.Now}");
}

public enum Unit
{ 
	Bytes = 0,
	KiloBytes = 10,
	MegaBytes = 20,
	GigaBytes = 30
}

public int GetFileSizeInBytes(Unit unit, int units)
{
	var power = (double)unit;
	
	var fileSize = Convert.ToInt32(Math.Pow(2, power) * units);
	
	return fileSize;
}

public void CreateDummyFiles(int numberOfFiles, int fileSizesInBytes, string saveTo, string saveAs, string extension)
{
	for (int i = 0; i < numberOfFiles; i++)
	{
		var file = saveAs + i + extension;
		
		var path = Path.Combine(saveTo, file);

		var txt = GetPayLoad(fileSizesInBytes);
		
		File.WriteAllText(path, txt);
	}
}

// Define other methods and classes here
public string GetPayLoad(int bytes)
{
	var r = new Random();
	
	var sb = new StringBuilder();
	
	var lineWidth = 0;
	
	for (int i = 0; i < bytes; i++)
	{
		lineWidth++;

		var n = r.Next(255); //ASCII limits

		var b = new byte[] { Convert.ToByte(n) };

		string s = Encoding.ASCII.GetString(b);

		sb.Append((char)n);

		if (lineWidth == 2000)
		{
			sb.AppendLine();
		}
	}
	
	var txt = sb.ToString();
	
	return txt;
}