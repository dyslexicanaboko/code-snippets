<Query Kind="Program" />

void Main()
{
	WindowsUserAddAsReadOnly("AD\\Username", "DatabaseA", "DatabaseB", "DatabaseC");
}

public void WindowsUserAddAsReadOnly(string userAdName, params string[] databases)
{
	var s = new StringBuilder();

	foreach (var d in databases)
	{
		s.AppendLine($@"
	USE {d}
	GO
	CREATE USER [{userAdName}] FOR LOGIN [{userAdName}]
	GO
	USE {d}
	GO
	ALTER ROLE [db_datareader] ADD MEMBER [{userAdName}]
	GO");
	}

	s.ToString().Dump();
}

// Define other methods and classes here
public void Original()
{
	/*	Give a new user read and write permissions with the dbo schema as default.
		You will have to re-enter the password for your user via the GUI after
		executing this script because you are supposed to provide a hash. So edit
		the properties of your user afterwards.
		
		Giving this user EXECUTE permissions on sprocs is a separate operation. */

	var dbs = new[] { "DatabaseA", "DatabaseB", "DatabaseC" };

	dbs.Dump();

	var user = "username";
	var pw = "password";

	var s = new StringBuilder($@"USE [master]
	GO
	CREATE LOGIN [{user}] WITH PASSWORD=N'{pw}', DEFAULT_DATABASE=[master], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF");
	s.AppendLine();
	s.AppendLine("GO");

	foreach (var d in dbs)
	{
		s.AppendLine($@"USE {d}
	GO
	CREATE USER [{user}] FOR LOGIN [{user}]
	GO
	USE {d}
	GO
	ALTER USER [{user}] WITH DEFAULT_SCHEMA=[dbo]
	GO
	USE {d}
	GO
	ALTER ROLE [db_datareader] ADD MEMBER [{user}]
	GO
	USE {d}
	GO
	ALTER ROLE [db_datawriter] ADD MEMBER [{user}]
	GO");
	}

	s.ToString().Dump();
}