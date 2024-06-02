<Query Kind="Program">
  <IncludeUncapsulator>false</IncludeUncapsulator>
</Query>

void Main()
{
	//Use the `gitStaleBranches.sh` script to produce a new line delimited file of branch names
	//Double check the branches listed to make sure they are not including branches you do not
	//want to delete.
	var path = @"C:\Dump\delete2\stale-branches.txt";
	
	var hs = GetBranches(path, ScrubList);
	
	//hs.Dump();
	DeleteRemoteBranches(hs);
}

//Copy this to an file and execute or paste into a command window
private void DeleteRemoteBranches(HashSet<string> branches)
{
	foreach (var b in branches)
	{
		Console.WriteLine($"git push origin --delete {b}");
	}
}

private HashSet<string> GetBranches(string path, string[] scrubList)
{
	var c = StringComparer.OrdinalIgnoreCase;
	
	var lines = File.ReadAllLines(path)
		.Where(x => !string.IsNullOrWhiteSpace(x))
		.Where(x => !scrubList.Contains(x, c));
	
	var hs = new HashSet<string>(lines, c);
		
	return hs;
}

//Generic branches that should never be deleted
private string[] ScrubList = new[] 
{
	"master",
	"develop",
	"main"
};

