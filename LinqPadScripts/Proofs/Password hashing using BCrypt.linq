<Query Kind="Program">
  <NuGetReference>BCrypt.Net</NuGetReference>
  <Namespace>BCrypt.Net</Namespace>
  <Namespace>Crypto = BCrypt.Net.BCrypt</Namespace>
</Query>

//BCrypt.Net is being used via NuGet
//using Crypto = BCrypt.Net.BCrypt;
void Main()
{
	HashPassword("a").Dump();
	
	IsPasswordValid("a", "$2a$12$uzHi7LlOFMLoWBVaAIvxne6YALE31XN7JVGkYH.KiHA.8vTQuup4K").Dump();
}

// Define other methods and classes here
public static string HashPassword(string plainTextPassword)
{
	var salt = Crypto.GenerateSalt(12);

	var hashedPassword = Crypto.HashPassword(plainTextPassword, salt);

	return hashedPassword;
}

public static bool IsPasswordValid(string password, string correctHash)
{
	var isValid = Crypto.Verify(password, correctHash);

	return isValid;
}