<Query Kind="Program" />

void Main()
{
	RemoveAppOffline();
}

List<string> _servers = new List<string>()
{
	"ServerA",
	"ServerB",
	"ServerC"
};

public void RemoveAppOffline()
{
	var clients = new List<string>()
	{
		"ClientA",
	};
	
	var sb = new StringBuilder();
	
	foreach (var s in _servers)
	{
		foreach (var c in clients)
		{
			sb
				.Append("DEL \"")
				.Append(@"\\{S}\E$\Applications\{C}\app_offline.htm"
					.Replace("{S}", s)
					.Replace("{C}", c))
				.AppendLine("\"");
		}
	}
	
	sb.ToString().Dump();
}