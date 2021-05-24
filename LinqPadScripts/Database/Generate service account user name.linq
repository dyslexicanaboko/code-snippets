<Query Kind="Program" />

const int MaxLength = 24;

void Main()
{
	var lst = new[]
	{
		"DatabaseA",
		"DatabaseB",
	};
	
	GetServiceAccounts(lst).Dump();
}

List<string> GetServiceAccounts(string[] databases)
{
	var lst = databases
		.Select(GetServiceAccountUserName)
		.ToList();
	
	return lst;
}

string GetServiceAccountUserName(string databaseName)
{
	var client = databaseName
		.Replace(@"Keyword1", string.Empty)
		.Replace(@"Keyword2", string.Empty)
		.Replace(@"Keyword3", string.Empty)
		.ToLower();

	var strAdUser = @"AD\UserName" + client;

	if (strAdUser.Length > MaxLength)
	{
		strAdUser = strAdUser.Substring(0, MaxLength);
	}

	return strAdUser;
}
