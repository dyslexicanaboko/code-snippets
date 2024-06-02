<Query Kind="Program" />

void Main()
{
	//Branches
	var repos = new List<Repo>
	{
		new Repo("T-100", "branch1", @"C:\dev\repo1"),
		new Repo("T-200", "branch2", @"C:\dev\repo2"),
		new Repo("T-300", "branch3", @"C:\dev\repo3"),
	};
	
	//CommitAndPush(repos, "Commit message here");
	
	//DeleteAndGetLatest(repos);
}

/// <summary>
/// Stage changes, commit changes with common message and push branch to remote
/// </summary>
public void CommitAndPush(List<Repo> repos, string commitMessage)
{
	foreach (var r in repos)
	{
		Console.WriteLine($@"git add -A
git commit -m ""{r.TicketNumber} {commitMessage}""
git push --set-upstream origin {r.Branch}
");
		Console.WriteLine();
	}
}

/// <summary>
/// Switch to develop, delete topic branch, get latest
/// </summary>
public void DeleteAndGetLatest(List<Repo> repos)
{
	foreach (var r in repos)
	{
		Console.WriteLine($@"cd {r.Directory}
git checkout develop
git branch -D {r.Branch}
git fetch --all
git remote prune origin
git pull
");
		Console.WriteLine();
	}
}

public class Repo
{
	public Repo(string ticketNumber, string branch, string directory)
	{
		TicketNumber = ticketNumber;
		Branch = branch;
		Directory = directory;
	}
	
	public string TicketNumber { get; set; }
	public string Branch { get; set; }
	public string Directory { get; set; }
}
