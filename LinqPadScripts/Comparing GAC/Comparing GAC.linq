<Query Kind="Program">
  <Namespace>System.Net.Mail</Namespace>
</Query>

const string gac1 = @"C:\Workspace\assembly old.txt";
const string gac2 = @"C:\Workspace\assembly new.txt";

void Main()
{
	Console.WriteLine("Start");
	
	List<string> lstGac1 = GetStringList(gac1);
	List<string> lstGac2 = GetStringList(gac2);

	//Bubble sort
	Console.WriteLine("List 1 Count {0}, List 2 Count {1}, Equal? {2}\n", lstGac1.Count, lstGac2.Count, lstGac1.Count == lstGac2.Count);

	Console.WriteLine("Is list 1 found in list 2?\n\n");
	
	foreach(var g1 in lstGac1)
	{
		if(!lstGac2.Contains(g1))
			Console.WriteLine("[{0}] was not found in list 2", g1);
	}

	Console.WriteLine("Is list 2 found in list 1?\n\n");
	
	foreach(var g2 in lstGac2)
	{
		if(!lstGac1.Contains(g2))
			Console.WriteLine("[{0}] was not found in list 1", g2);
	}

	Console.WriteLine("End");
}

public class GacFiles
{
	public string FullFilePath { get; set; }
	public string FileName { get; set; }
}


public List<string> GetStringList(string targetFullFilePath)
{
	var lst = new List<string>();
	
	using(var sr = new StreamReader(targetFullFilePath))
	{
		string strLine = null;
	
		while(!sr.EndOfStream)
		{
			strLine = sr.ReadLine();
			
			if(string.IsNullOrWhiteSpace(strLine))
				continue;
				
			lst.Add(strLine);
		}
	}
	
	return lst;
}