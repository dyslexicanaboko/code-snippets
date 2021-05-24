<Query Kind="Program">
  <Namespace>System.Security.Cryptography</Namespace>
</Query>

/*
	Example of generating password hashes
*/
void Main()
{
	var username = "username";

	var hashedPw = GenerateFsPasswordHash(username, "FyP6y6CsarEhM67aXrHf").Dump(); //hqbRIusK/kCM+HQfxoI+Irepni9FtXEUlG3mv2/kHmwPngrMQAAMrruASBHaWPz2
	
	//This is the junk password I use to meet the requirements
	//aA12345
	
	var sql = GetPwUpdateSql(username, hashedPw);

	Console.WriteLine();
	Console.WriteLine(sql);
}

private string GetPwUpdateSql(string userName, string hashedPassword)
{
	return 
@"UPDATE dbo.User SET
	PasswordHash = '" + hashedPassword + @"'
WHERE UserName = '" + userName + "'";
}

private static string GenerateFsPasswordHash(string userName, string password)
{
	Console.WriteLine($"UN: {userName}\n PW: {password}");
	
	userName = userName.Trim();

	return Convert.ToBase64String(new SHA384Managed()
		.ComputeHash(new UnicodeEncoding()
			.GetBytes($"{password}.{userName}")));
}