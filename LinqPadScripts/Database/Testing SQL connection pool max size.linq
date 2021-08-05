<Query Kind="Program">
  <Namespace>System.Threading.Tasks</Namespace>
</Query>

void Main()
{
	OpenConnectionsInSerial(5, 6);
	//OpenConnectionsInParallel(5, 10);
}

string _connectionStringBase = $"Server=.;Database=master;Integrated Security=SSPI;";

private string AddMaxPoolSize(int maxPoolSize)
{
	var cb = new SqlConnectionStringBuilder(_connectionStringBase);
	cb.MaxPoolSize = maxPoolSize;

	var strCon = cb.ToString();
	strCon.Dump();
	
	return strCon;
}

private void OpenConnectionsInParallel(int maxPoolSize, int threadCount)
{
	var strCon = AddMaxPoolSize(maxPoolSize);

	Parallel.For(1, threadCount + 1, (i) => OpenConnection(strCon, i));
}

//Synchronous run of trying to open connections in a loop
private void OpenConnectionsInSerial(int maxPoolSize, int loopCount)
{
	var strCon = AddMaxPoolSize(maxPoolSize);

	for (int i = 1; i <= loopCount; i++)
	{
		OpenConnection(strCon, i);
	}
}

private void OpenConnection(string connectionString, int connectionNumber)
{	
	Console.WriteLine($"Con#{connectionNumber}");
	
	try
	{	        
		var con = new SqlConnection(connectionString);
		con.Open();
	
		using (var cmd = new SqlCommand($"SELECT CONCAT('KILL ', CAST(@@SPID AS VARCHAR(10))) AS KillCmd", con))
		{
			using (var dr = cmd.ExecuteReader())
			{
				while (dr.Read())
				{
					var killCmd = Convert.ToString(dr["KillCmd"]);
	
					Console.WriteLine($"{killCmd} --{connectionNumber}");
				}
			}
		}
	}
	catch (Exception ex)
	{
		Console.WriteLine($"{connectionNumber}: {ex.Message}");
	}
}