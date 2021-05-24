<Query Kind="Program" />

string INPUT_BRANCH_TASKS; //Screen scrapped data
string INPUT_BRANCH_BUGS; //Screen scrapped data
string INPUT_LONG_COMMIT; //Long commit pool of data
string BASE_PATH;
/*
If you copy and paste the text from branches screen in TFS it follows a pattern like this:

00 1
01 New pull request
02 branch name
03 a22c4e15
04 User name
05 4/24/2019
06 294
07  
08 1
09 New pull request
10 branch name
11 72997d61
12 username
13 10/10/2018
14 899

The key index is index 2 and the next index is 7 lines below that
If "New pull request" is missing from the input just add it in for where it is missing. */
void Main()
{
	BASE_PATH = Path.GetDirectoryName(Util.CurrentQueryPath);

	INPUT_BRANCH_TASKS = Path.Combine(BASE_PATH, "input-tasks.txt");
	INPUT_BRANCH_BUGS = Path.Combine(BASE_PATH, "input-bugs.txt");
	INPUT_LONG_COMMIT = Path.Combine(BASE_PATH, "input-longCommitLookup.txt");

	var lstTasks = GetScrapedData(INPUT_BRANCH_TASKS, @"tasks");
	var lstBugs = GetScrapedData(INPUT_BRANCH_BUGS, @"bugs");

	var lstScraped = new List<Scraped>(lstTasks.Count + lstBugs.Count);

	lstScraped.AddRange(lstTasks);
	lstScraped.AddRange(lstBugs);

	//var dictCommits = GetLongCommitPool(INPUT_LONG_COMMIT);
	
	//MatchLongCommitToShort(lstScraped, dictCommits);

	//dictCommits.Dump();
	lstScraped.Dump();
}

private void MatchLongCommitToShort(List<Scraped> scrapedData, Dictionary<string, string> longCommits)
{
	//Sorting the scraped data by commit
	var lstSorted = scrapedData.OrderBy(x => x.ShortCommit).ToList();
	
	//Attempt to match each value
	for (var i = 0; i < lstSorted.Count; i++)
	{
		var s = lstSorted[i];
		
		if(!longCommits.TryGetValue(s.ShortCommit, out var longCommit)) continue;
		
		lstSorted[i].DerrivedLongCommit = longCommit;
	}
}

/* 	This is a list of long commit IDs pulled using:
		git rev-parse --remotes
	This pulls all commits I have not found a way to filter it yet even though there is a switch for it
	Switch doesn't work as far as I can tell */
private Dictionary<string, string> GetLongCommitPool(string fullFilePath)
{
	//Values from the git command are sometimes duplicated
	var lines = File.ReadAllLines(fullFilePath)
		.Distinct()
		.Select(x => x.ToLower())
		.OrderBy(x => x)
		.ToArray();
	
	var dict = new Dictionary<string, string>(lines.Length);
	
	//Get the short commits immediately by getting the first 8 characters of the long commit
	foreach (var l in lines)
	{
		var key = l.Substring(0, 8);

		if (dict.ContainsKey(key))
		{
			Console.WriteLine($"Long commit: {l} clashes with short commit {key}");
			
			continue;
		}
		
		dict.Add(key, l);
	}

	Console.WriteLine($"Long commit pool size: {dict.Count}");

	return dict;
}

private void GenerateTagCreationCommands(List<Scraped> scrapedData)
{
	for (var i = 0; i < scrapedData.Count; i++)
	{
		var s = scrapedData[i];

		var cmd = $"";

		scrapedData[i].DerrivedCommand = cmd;
	}
}

private void GetWorkItems(List<string> branches)
{
	var lstWorkItems = GetPrefixedWorkItems(branches);

	Console.WriteLine($"Entries with work item prefix: {lstWorkItems.Count}");

	var csv = string.Join(",", lstWorkItems);

	Console.WriteLine($"CSV of work items:\n\t{csv}");

	Console.WriteLine("\nWork items founds");

	lstWorkItems.Dump();
}

private List<int> GetPrefixedWorkItems(List<string> branches)
{
	var sb = new StringBuilder();

	var r = new Regex(@"([\d]+).+");

	var lstWorkItems = branches.Select(x =>
	{
		var m = r.Match(x);

		if (!m.Success) return 0;
		
		var intValue = Convert.ToInt32(m.Groups[1].Value);
		
		return intValue;
	})
	.Where(x => x != 0)
	.Distinct()
	.OrderBy(x => x)
	.ToList();

	return lstWorkItems;
}

private const int NextBranchOffset = 7;

private List<Scraped> GetScrapedData(string fullFilePath, string folder)
{
	var lines = File.ReadAllLines(fullFilePath);

	var lst = new List<Scraped>(lines.Length);

	//Start at index 2 because that's where the first branch shows up
	for (int i = 2; i < lines.Length; i++)
	{
		//Index 0 and 1 are garbage skip them
		var s = new Scraped
		{
			BranchName = lines[i],
			ShortCommit = lines[i + 1].ToLower(),
			LastAuthor = lines[i + 2],
			Date = lines[i + 3],
		};

		s.Folder = folder;

		lst.Add(s);

		//Move the pointer to the next glob of data
		i += NextBranchOffset;
	}

	lst.TrimExcess();

	lst = lst.OrderBy(x => x.BranchName).ToList();

	Console.WriteLine($"Total number of {folder} branches: {lst.Count}");

	return lst;
}

public class Scraped
{
	//These are what show up on the screen
	//0 Ignore
	//1 Ignore
	public string BranchName { get; set; }    //2
	public string ShortCommit { get; set; }   //3
	public string LastAuthor { get; set; }    //4
	public string Date { get; set; }          //5
											  //6 Ignore

	//User provided
	public string Folder { get; set; }

	//Optionally derrived
	public string DerrivedLongCommit { get; set; }
	public string DerrivedCommand { get; set; }
}